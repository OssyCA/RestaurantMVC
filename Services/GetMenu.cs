using RestaurantMVC.Models;
using RestaurantMVC.Services.IGetMenuService;

namespace RestaurantMVC.Services
{
    public class GetMenu(IHttpClientFactory httpClientFactory) : IGetMenu
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");
        public async Task<List<MenuItem>> Menu()
        {
            var response = await _httpClient.GetAsync("Menu/GetWholeMenu");

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuItem>>>();
            if (apiResponse == null || apiResponse.Data == null)
            {
                return [];
            }

            return apiResponse.Data;
        }
    }
}
