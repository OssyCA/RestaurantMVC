using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestaurantMVC.Controllers
{
    public class HomeController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Menu/GetWholeMenu");


            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuItem>>>();
            var menu = apiResponse?.Data;
            return View(menu);
        }
    }
}
