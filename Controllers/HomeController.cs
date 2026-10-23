using GlassDesk.Data;
using GlassDesk.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace GlassDesk.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoDbContext _dbContext;

        // Constructor Dependency Injection
        public HomeController(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Ping the MongoDB server to confirm active connection
                await _dbContext.Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                ViewBag.DbStatus = "Connected";
                ViewBag.DbName = _dbContext.Database.DatabaseNamespace.DatabaseName;
            }
            catch (Exception ex)
            {
                ViewBag.DbStatus = $"Connection Error: {ex.Message}";
                ViewBag.DbName = _dbContext.Database.DatabaseNamespace.DatabaseName;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}