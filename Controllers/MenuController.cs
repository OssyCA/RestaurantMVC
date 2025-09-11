using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

            var menuItems = await menu.Menu();
            var manageModel = new ManageMenuVM
            {
                MenuItems = menuItems,
                NewMenuItem = model
            };
            if (!ModelState.IsValid)
            {

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
                return View("ManageMenuItems", manageModel);
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, MenuItemVM model)
        {
            ViewBag.SelectedId = id;

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ManageMenuItems");
            }

            var dto = new UpdateMenuItemDTO
            {
                Id = id,
                Title = model.Title,
                Description = model.Description,
                IsPopular = model.IsPopular,
                ImageUrl = model.ImageUrl ?? "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAQAAAADFCAMAAACM/tznAAAAbFBMVEUAAAD////Pz8/Hx8empqasrKx6enrExMT39/dubm4LCwsQEBC3t7ewsLCgoKCXl5eIiIguLi7u7u6Li4uCgoLf39/W1tYVFRW0tLRnZ2ciIiJ0dHRgYGCamprZ2dnn5+dNTU1ISEg7OztYWFg9GncPAAAB8klEQVR4nO3Z23KCMBSF4WzUQmsQq3io2oP6/u/YBAPEKcWLTiNj/u/GJALbLMZiE6UAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADi9eILXXEapmKvVLKa6CAVF1lbcRakYq+RtEZBKnoFZRykYi8TwLacV5ZvQSq6avMyGUoA+zuVngwlgKc7lSaAwQbwkmo9qTu7VOeuWWrdfF1M+7k5IdVp17V/Ga4NN4Cp/QN9cJ1Xkcw1t94n1iLvzQnm8K5r/zJcG3gAK9fxAsilfXCbAIrmhAcMIBNxD8VIA9Af8nnpRBpA8lX/Pog0gJVayaLqRBrAQp3dG5EGYOa8vkw80gDMJz+KlCrmAFRSzTziAE4iy6gDsO/FHcCrneQm4gBUal9jDmBn/+WLOQA7zesA6qUB84iIIgDTKtoACnE/j1U1uYPyOl3XfoAA1FayrAnAzsjd9vRqcg8cQLWK3wRQ2IWScTHLM2nWC9xBaWtU9g/XBhNAqTYXUzfmB5D7AahZu6fhL6bLtbx32BXb2EsPI4AfO0PTdZI0B5i2t2N00vbmy2J8tav3kfjWy97hge0MJR0B3GBu3l/4AUxuH/7fzs+tc5CKXsH9MUhFAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALizb0qhFsAfTxmyAAAAAElFTkSuQmCC",
                Price = model.Price
            };

            var response = await _httpClient.PutAsJsonAsync($"Menu/UpdateItem/{id}", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Menyobjektet har uppdaterats!";
                return RedirectToAction("ManageMenuItems");
            }

            TempData["ErrorMessage"] = "Ett fel uppstod när meny-objektet skulle uppdateras.";
            return RedirectToAction("ManageMenuItems");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"Menu/DeleteItem/{id}");

            if (!response.IsSuccessStatusCode)
            {
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