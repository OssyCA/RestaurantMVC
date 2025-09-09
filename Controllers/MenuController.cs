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
            var menuItems = await menu.Menu(); // Make sure this is async
            ViewBag.ShowEditButtons = true;
            ViewBag.ShowDeleteButtons = true;
            ViewBag.ShowDescriptionBtn = false;
            return View("Index", menuItems); // This should pass List<MenuItem>
        }
        public IActionResult CreateMenuItem()
        {
            var model = new MenuItemVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMenuItemAsync(MenuItemVM model)
        {
            if (!ModelState.IsValid)  
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToList();

                ViewBag.ValidationErrors = errors;
                return View("ManageMenuItems", model);  
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
            return View("ManageMenuItems");  
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

            return RedirectToAction("Index");

        }

    }
}
