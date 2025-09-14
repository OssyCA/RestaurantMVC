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
            return RedirectToAction("Index", "Table");
        }
        public IActionResult HandleBooknings()
        {
            return RedirectToAction("ManageBookings", "Booking");
        }
        public IActionResult HandleMenu()
        {
            return RedirectToAction("ManageMenuItems", "Menu");
        }
    }
}
