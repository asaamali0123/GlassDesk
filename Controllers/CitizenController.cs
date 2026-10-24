using GlassDesk.Data;
using GlassDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;

namespace GlassDesk.Controllers
{
    [Authorize(Roles = UserRoles.Citizen)]
    public class CitizenController : Controller
    {
        private readonly IMongoCollection<Complaint> _complaintsCollection;

        public CitizenController(IMongoDbContext dbContext)
        {
            _complaintsCollection = dbContext.GetCollection<Complaint>("complaints");
        }

        public async Task<IActionResult> Index()
        {
            var citizenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var myComplaints = new List<Complaint>();
            if (!string.IsNullOrEmpty(citizenId))
            {
                myComplaints = await _complaintsCollection
                    .Find(c => c.CitizenId == citizenId)
                    .SortByDescending(c => c.CreatedAt)
                    .ToListAsync();
            }

            return View(myComplaints);
        }

        // Sub-page: Submit Complaint Form
        [HttpGet]
        public IActionResult Submit()
        {
            return View();
        }
    }
}