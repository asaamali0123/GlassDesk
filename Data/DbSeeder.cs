using GlassDesk.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace GlassDesk.Data
{
    public static class DbSeeder
    {
        public static async Task SeedUsersAsync(IMongoDbContext dbContext)
        {
            var usersCollection = dbContext.GetCollection<User>("users");
            var passwordHasher = new PasswordHasher<User>();
            const string defaultPassword = "GlassDesk#2026"; // 14 characters, strong

            var seedList = new List<User>
            {
                // 1. System Administrator
                new User
                {
                    FullName = "System Administrator",
                    Email = "admin@glassdesk.local",
                    PhoneNumber = "0300-1111111",
                    Role = UserRoles.Administrator,
                    Department = null,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                // 2. Verification Officer (Central Intake / Triage)
                new User
                {
                    FullName = "Verification Officer Tariq",
                    Email = "verifier@glassdesk.local",
                    PhoneNumber = "0300-2222222",
                    Role = UserRoles.VerificationOfficer,
                    Department = "Central Verification",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                // 3. Department Officer (Staff Member)
                new User
                {
                    FullName = "Engr. Usman (Officer)",
                    Email = "officer.water@glassdesk.local",
                    PhoneNumber = "0300-3333333",
                    Role = UserRoles.DepartmentOfficer,
                    Department = ComplaintDepartments.WaterAndSanitation,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                // 4. Department Supervisor
                new User
                {
                    FullName = "Director Fatima (Supervisor)",
                    Email = "supervisor.water@glassdesk.local",
                    PhoneNumber = "0300-4444444",
                    Role = UserRoles.Supervisor,
                    Department = ComplaintDepartments.WaterAndSanitation,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                // 5. Citizen
                new User
                {
                    FullName = "Citizen Ahmad Khan",
                    Email = "citizen@glassdesk.local",
                    PhoneNumber = "0300-5555555",
                    Role = UserRoles.Citizen,
                    Department = null,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            foreach (var user in seedList)
            {
                var existingUser = await usersCollection
                    .Find(u => u.Email.ToLower() == user.Email.ToLower())
                    .FirstOrDefaultAsync();

                if (existingUser == null)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, defaultPassword);
                    await usersCollection.InsertOneAsync(user);
                }
            }
        }
    }
}