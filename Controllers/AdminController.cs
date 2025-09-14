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
        [Authorize]
        public IActionResult HandleTable()
        {
            return RedirectToAction("Index", "Table");
        }
        [Authorize]
        public IActionResult HandleBooknings()
        {
            return RedirectToAction("ManageBookings", "Booking");
        }
        [Authorize]
        public IActionResult HandleMenu()
        {
            return RedirectToAction("ManageMenuItems", "Menu");
        }
    }
}
