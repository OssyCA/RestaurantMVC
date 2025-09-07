using Microsoft.Extensions.Logging;
using RestaurantMVC.Models;
using RestaurantMVC.Services.IGetMenuService;

namespace RestaurantMVC.Services
{
    public class GetMenu(IHttpClientFactory httpClientFactory, ILogger<GetMenu> _logger) : IGetMenu
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");
        public async Task<List<MenuItem>> Menu()
        {
            try
            {
                var response = await _httpClient.GetAsync("Menu/GetWholeMenu");

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuItem>>>();
                if (apiResponse == null || apiResponse.Data == null)
                {
                    return [];
                }

                return apiResponse.Data;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning($"API not available {ex.Message}");
                return GetFallbackMenu();
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.Message}");
                return GetFallbackMenu();
            }

        }

        private static List<MenuItem> GetFallbackMenu()
        {
            return [
                new() {
                    Title = "Not found",
                    Description = "Not found"
                }
            ];
        }
    }
}
