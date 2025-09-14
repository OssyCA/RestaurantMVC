using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RestaurantMVC.Models;
using RestaurantMVC.Services.Iservices;

namespace RestaurantMVC.Services
{
    public class GetMenu(IHttpClientFactory httpClientFactory, ILogger<GetMenu> _logger, IMemoryCache _cache) : IGetMenu
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");
        private const string MENU_CACHE_KEY = "restaurant_menu";
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30); // Cacha i 30 minuter

        public async Task<List<MenuItem>> Menu()
        {
            if (_cache.TryGetValue(MENU_CACHE_KEY, out List<MenuItem>? cachedMenu))
            {
                _logger.LogInformation("Menu getted from cache");
                return cachedMenu ?? [];
            }
            try
            {
                _logger.LogInformation("Getting API");
                var response = await _httpClient.GetAsync("Menu/GetWholeMenu");

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuItem>>>();
                if (apiResponse == null || apiResponse.Data == null)
                {
                    return [];
                }


                var menu = apiResponse.Data;

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration,
                    SlidingExpiration = TimeSpan.FromMinutes(10),  
                    Priority = CacheItemPriority.High 
                };

                _cache.Set(MENU_CACHE_KEY, menu, cacheOptions);
                _logger.LogInformation($"Menu cachad i {_cacheDuration.TotalMinutes} minuter");

                return menu;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "API not available: {Message}", ex.Message);
                return GetFallbackMenu();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred: {Message}", ex.Message);
                return GetFallbackMenu();
            }

        }
        public void ClearMenuCache()
        {
            _cache.Remove(MENU_CACHE_KEY);
            _logger.LogInformation("Menu cache cleared");
        }

        private static List<MenuItem> GetFallbackMenu()
        {
            return new List<MenuItem>
            {
                new MenuItem
                {
                    Title = "Menu not available",
                    Description = "Could not load menu from API"
                }
            };
        }

    }
}
