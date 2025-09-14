using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.DTOs;
using RestaurantMVC.Models;
using RestaurantMVC.Services;
using RestaurantMVC.Services.Iservices;
using RestaurantMVC.ViewModels;
using System.Reflection;

namespace RestaurantMVC.Controllers
{
    public class TableController(IGetTables getTables, IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");
        public async Task<IActionResult> Index()
        {
            var manangeTable = new ManageTableVM
            {
                NewTable = new TableVM(),
                Tables = await getTables.GetAllTables()

            };
            return View(manangeTable);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTable(TableVM model)
        {
            var manangeTable = new ManageTableVM
            {
                NewTable = model,
                Tables = await getTables.GetAllTables()

            };
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            var modelDto = new CreateTableDTO
            {
                TableNumber = model.TableNumber,
                Capacity = model.Capacity,
                BookingLocked = false
            };

            var response = await _httpClient.PostAsJsonAsync("Table/CreateTable", modelDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Ett fel uppstod när meny-objektet skulle sparas.");
                return View("Index", manangeTable);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var response = await _httpClient.DeleteAsync($"Table/DeleteTable/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Ett fel uppstod när bordet skulle tas bort.";
            }
            else
            {
                TempData["SuccessMessage"] = "Bordet har tagits bort.";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TableVM tableVM)
        {
            ViewBag.SelectedId = id;

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var dto = new UpdateTableDTO
            {
                Id = id,
                Capacity = tableVM.Capacity,
                TableNumber = tableVM.TableNumber,
                BookingLocked = false
            };

            var response = await _httpClient.PutAsJsonAsync($"Table/UpdateTable/{id}", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Table har uppdaterats!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Ett fel uppstod bordet skulle uppdateras.";
            return RedirectToAction("Index");
        }
    }
}
