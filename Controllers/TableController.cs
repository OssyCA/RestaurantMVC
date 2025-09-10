using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.Services;
using RestaurantMVC.ViewModels;

namespace RestaurantMVC.Controllers
{
    public class TableController(GetTables getTables) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var mangeTable = new ManageTableVM
            {
                NewTable = new Models.RestaurantTable(),
                Tables = await getTables.GetAllTables()

            };
            return View(mangeTable);
        }
    }
}
