using GlassDesk.Data;
using GlassDesk.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;

namespace GlassDesk.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(IMongoDbContext dbContext)
        {
            _usersCollection = dbContext.GetCollection<User>("users");
            _passwordHasher = new PasswordHasher<User>();
        }

        // ==========================================
        // 1. CITIZEN REGISTRATION (GET & POST)
        // ==========================================

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _usersCollection
                .Find(u => u.Email.ToLower() == model.Email.Trim().ToLower())
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "An account with this email address already exists.");
                return View(model);
            }

            var newUser = new User
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim().ToLower(),
                PhoneNumber = model.PhoneNumber.Trim(),
                Role = UserRoles.Citizen, // Discussion 19: Citizens self-register strictly as Citizen
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);

            await _usersCollection.InsertOneAsync(newUser);

            TempData["SuccessMessage"] = "Registration successful! You can now log in with your credentials.";
            return RedirectToAction("Login", new { role = UserRoles.Citizen });
        }

        // ==========================================
        // 2. USER LOGIN (GET & POST)
        // ==========================================

        [HttpGet]
        public IActionResult Login(string? role = null, string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel
            {
                SelectedRole = role,
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _usersCollection
                .Find(u => u.Email.ToLower() == model.Email.Trim().ToLower())
                .FirstOrDefaultAsync();

            if (user == null || !user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            // Discussion 19: Server-side Role Authorization Check
            if (!string.IsNullOrWhiteSpace(model.SelectedRole) &&
                !string.Equals(model.SelectedRole, "Any", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(user.Role, model.SelectedRole, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty,
                    $"Access Denied: This account is registered as '{user.Role}', not '{model.SelectedRole}'.");
                return View(model);
            }

            // Build Claims for the authenticated session cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Department", user.Department ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            // Route user directly to their role home/dashboard
            return user.Role switch
            {
                UserRoles.Citizen => RedirectToAction("Index", "Citizen"),
                UserRoles.VerificationOfficer => RedirectToAction("Index", "VerificationOfficer"),
                UserRoles.DepartmentOfficer => RedirectToAction("Index", "DepartmentOfficer"),
                UserRoles.Supervisor => RedirectToAction("Index", "Supervisor"),
                UserRoles.Administrator => RedirectToAction("Index", "Admin"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        // ==========================================
        // 3. LOGOUT
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // 4. ACCESS DENIED
        // ==========================================

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}