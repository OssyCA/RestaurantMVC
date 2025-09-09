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
            ViewBag.SelectedId = selectedId;
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
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMenuItem(MenuItemVM model)
        {
            if (!ModelState.IsValid)
            {
                var menuItems = await menu.Menu();
                var manageModel = new ManageMenuVM
                {
                    MenuItems = menuItems,
                    NewMenuItem = model 
                };

                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToList();

                ViewBag.ValidationErrors = errors;
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

            var response = await _httpClient.PostAsJsonAsync("Menu/CreateMenuItem", dto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ManageMenuItems");
            }
            else
            {
                ModelState.AddModelError("", "Ett fel uppstod när meny-objektet skulle sparas.");
                var menuItems = await menu.Menu();
                var manageModel = new ManageMenuVM
                {
                    MenuItems = menuItems,
                    NewMenuItem = model
                };
                return View("ManageMenuItems", manageModel);
            }
        }

        public async Task<IActionResult> Edit()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MenuItemVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new UpdateMenuItemDTO
            {
                Id = id,
                Title = model.Title,
                Description = model.Description,
                IsPopular = model.IsPopular,
                ImageUrl = model.ImageUrl,
                Price = model.Price
            };

            var response = await _httpClient.PutAsJsonAsync($"Menu/UpdateItem/{id}", dto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ManageMenuItems");
            }

            ModelState.AddModelError("", "Ett fel uppstod när meny-objektet skulle uppdateras.");
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"Menu/DeleteItem/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Du kan lägga till TempData för felmeddelanden här
                TempData["ErrorMessage"] = "Ett fel uppstod när meny-objektet skulle tas bort.";
            }
            else
            {
                TempData["SuccessMessage"] = "Meny-objektet har tagits bort.";
            }

            return RedirectToAction("ManageMenuItems");
        }
    }
}