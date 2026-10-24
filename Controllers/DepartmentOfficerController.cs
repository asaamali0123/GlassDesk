using GlassDesk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlassDesk.Controllers
{
    [Authorize(Roles = UserRoles.DepartmentOfficer)]
    public class DepartmentOfficerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Sub-page: Department Work Orders & Resolution Submission
        [HttpGet]
        public IActionResult WorkOrders()
        {
            return View();
        }
    }
}