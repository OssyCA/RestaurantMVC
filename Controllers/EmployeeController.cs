using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.ViewModels;
using System.Threading.Tasks;

namespace RestaurantMVC.Controllers
{
    public class EmployeeController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginEmployee)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/LoginEmployee", loginEmployee);

            if (response.IsSuccessStatusCode)
            {
                return View(loginEmployee);
            }



            return View();
        }

    }
}
