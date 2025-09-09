using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantMVC.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HandleTable()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult HandleBooknings()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult HandleMenu()
        {
            return RedirectToAction("ManageMenuItems", "Menu");
        }
    }
}
