using GlassDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlassDesk.Controllers
{
    [Authorize(Roles = UserRoles.VerificationOfficer)]
    public class VerificationOfficerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Sub-page: Verification, Priority Triage & Duplicate Review
        [HttpGet]
        public IActionResult Triage()
        {
            return View();
        }
    }
}