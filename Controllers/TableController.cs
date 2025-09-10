using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.Services;

namespace RestaurantMVC.Controllers
{
    public class TableController(GetTables getTables) : Controller
    {
        public IActionResult Index()
        {
            return View(getTables.GetAllTables());
        }
    }
}
