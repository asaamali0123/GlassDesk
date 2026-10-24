using GlassDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlassDesk.Controllers
{
    [Authorize(Roles = UserRoles.Supervisor)]
    public class SupervisorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Sub-page: Escalation, Dispute & Reopening Management
        [HttpGet]
        public IActionResult Escalations()
        {
            return View();
        }
    }
}