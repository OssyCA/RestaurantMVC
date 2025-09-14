using RestaurantMVC.Models;
using RestaurantMVC.Services.Iservices;
using System.Net.Http;

namespace RestaurantMVC.Services
{
    public class GetTables(IHttpClientFactory httpClientFactory, ILogger<GetMenu> _logger): IGetTables
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");

        public async Task<List<RestaurantTable>> GetAllTables()
        {
            try
            {
                var response = await _httpClient.GetAsync("Table/GetAllTables");

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<RestaurantTable>>>();
                if (apiResponse == null || apiResponse.Data == null)
                {
                    return [];
                }

                return apiResponse.Data;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "API not available: {Message}", ex.Message);
                return FoundNoTables();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred: {Message}", ex.Message);
                return FoundNoTables();
            }

        }
        private static List<RestaurantTable> FoundNoTables()
        {
            return [
                new() {
                   
                }
            ];
        }
    }
}
