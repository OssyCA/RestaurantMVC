using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.CookiesOptions;
using RestaurantMVC.Models;
using RestaurantMVC.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            var response = await _httpClient.PostAsJsonAsync("Auth/LoginEmployee", loginEmployee);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid login attempt");  // FIXA BÄTTRE
                return View(loginEmployee); // FIxa error sida
            }
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TokenResponse>>();
            var jwt = apiResponse.Data.AccessToken;

            var handler = new JwtSecurityTokenHandler();
            var jwtObject = handler.ReadJwtToken(jwt);

            var claims = jwtObject.Claims.ToList();

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrinciple, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = jwtObject.ValidTo
                });
          

            if (apiResponse?.Data != null)
            {
                Response.Cookies.Append("accessToken", apiResponse.Data.AccessToken, GetCookieOptionsData.AccessTokenCookie());
                Response.Cookies.Append("refreshToken", apiResponse.Data.RefreshToken, GetCookieOptionsData.RefreshTokenCookie());
            }
            return RedirectToAction("Index", "Home");
            
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("accessToken");
            HttpContext.Response.Cookies.Delete("refreshToken");

            return RedirectToAction("Index", "Home");
        }

    }
}
