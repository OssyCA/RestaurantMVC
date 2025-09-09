using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.Models;
using RestaurantMVC.Services.Iservices;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestaurantMVC.Controllers
{
    public class HomeController(IGetMenu menu) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var items = await menu.Menu();



            return View(items.Where(i => i.IsPopular == true).ToList());
        }
    }
}
