namespace RestaurantMVC.Services
{
    public class GetBookings(IHttpClientFactory httpClientFactory)
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");

    }
}
