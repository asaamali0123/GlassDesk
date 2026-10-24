using GlassDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlassDesk.Controllers
{
    [Authorize(Roles = UserRoles.Administrator)]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Sub-page 1: Staff Account Provisioning & Governance
        [HttpGet]
        public IActionResult Staff()
        {
            return View();
        }

        // Sub-page 2: Dedicated Append-Only System Audit Logs
        [HttpGet]
        public IActionResult AuditLogs()
        {
            return View();
        }
    }
}