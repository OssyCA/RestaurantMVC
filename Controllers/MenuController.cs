using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.Models;
using RestaurantMVC.Services.IGetMenuService;
using System.Net.Http;

namespace RestaurantMVC.Controllers
{
    public class MenuController(IGetMenu menu) : Controller
    {
        public async Task<IActionResult> Index(int? selectedId = null)
        {
            var menuItems = await menu.Menu();

            ViewBag.SelectedId = selectedId; // fixa viewModel ist

            return View(menuItems);
        }

    }
}
