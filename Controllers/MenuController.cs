using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.DTOs;
using RestaurantMVC.Models;
using RestaurantMVC.Services.Iservices;
using RestaurantMVC.ViewModels;
using System.Net.Http;

namespace RestaurantMVC.Controllers
{
    public class MenuController(IGetMenu menu, IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");
        public async Task<IActionResult> Index(int? selectedId = null)
        {
            ViewBag.ShowDescriptionBtn = true;
            var menuItems = await menu.Menu();

            ViewBag.SelectedId = selectedId; // fixa viewModel ist

            return View(menuItems);
        }
        public async Task<IActionResult> ManageMenuItems()
        {
            var menuItems = await menu.Menu();
            var viewModel = new ManageMenuVM
            {
                MenuItems = menuItems,
                NewMenuItem = new MenuItemVM()
            };

            ViewBag.ShowEditButtons = true;
            ViewBag.ShowDeleteButtons = true;
            ViewBag.ShowDescriptionBtn = false;

            return View("ManageMenuItems", viewModel); 
        }
        public IActionResult CreateMenuItem()
        {
            var model = new MenuItemVM();
            return View(model);
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CreateMenuItem(MenuItemVM model)
        {
            if (!ModelState.IsValid)
            {
                // Vid fel: Skapa ManageMenuVM med felaktiga data
                var menuItems = await menu.Menu();
                var manageModel = new ManageMenuVM
                {
                    MenuItems = menuItems,
                    NewMenuItem = model // Behåll användarens input
                };

                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToList();

                ViewBag.ValidationErrors = errors;
                ViewBag.ShowEditButtons = true;
                ViewBag.ShowDeleteButtons = true;
                ViewBag.ShowDescriptionBtn = false;

                return View("ManageMenuItems", manageModel);
            }

            var dto = new CreateMenuItemDTO
            {
                Title = model.Title,
                Description = model.Description,
                IsPopular = model.IsPopular,
                ImageUrl = model.ImageUrl,
                Price = model.Price
            };

            await _httpClient.PostAsJsonAsync("Menu/CreateMenuItem", dto);

            // Vid framgång: Skapa ny ManageMenuVM med uppdaterad meny
            var updatedMenuItems = await menu.Menu();
            var successModel = new ManageMenuVM
            {
                MenuItems = updatedMenuItems,
                NewMenuItem = new MenuItemVM() // Tomt formulär
            };

            ViewBag.ShowEditButtons = true;
            ViewBag.ShowDeleteButtons = true;
            ViewBag.ShowDescriptionBtn = false;

            return View("ManageMenuItems", successModel);
        }
        public async Task<IActionResult> Update(int id)
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult Update(MenuItemVM model)
        {

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"Menu/DeleteItem/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("ManageMenuItems");

            return RedirectToAction("ManageMenuItems"); 
        }

    }
}
