// Services/GetBookings.cs
using RestaurantMVC.DTOs;
using RestaurantMVC.Models;
using RestaurantMVC.Services.Iservices;
using RestaurantMVC.ViewModels;

namespace RestaurantMVC.Services
{
    public class GetBookings(IHttpClientFactory httpClientFactory, ILogger<GetBookings> logger) : IGetBookings
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");

        public async Task<List<BookingVM>> GetAllBookings()
        {
            try
            {
                var response = await _httpClient.GetAsync("Booking/AllBookings");

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<Booking>>>();
                if (apiResponse == null || apiResponse.Data == null)
                {
                    return GetFallbackBookings();
                }

                var bookings = apiResponse.Data.Select(b => new BookingVM
                {
                    BookingId = b.Id,
                    CustomerName = b.CustomerName,
                    CustomerEmail = b.CustomerEmail,
                    CustomerPhone = b.CustomerPhone,
                    TableId = b.TableId,
                    TableNumber = b.TableNumber,
                    StartAt = b.StartTime,
                    ExpireAt = b.StartTime.AddHours(2),
                    Amount = b.Amount
                }).ToList();

                return bookings;
            }
            catch (HttpRequestException ex)
            {
                logger.LogWarning(ex, "API not available: {Message}", ex.Message);
                return GetFallbackBookings();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching bookings: {Message}", ex.Message);
                return GetFallbackBookings();
            }
        }

        public async Task<UpdateBookingVM?> GetBookingForEdit(int bookingId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Booking/GetBooking/{bookingId}");

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UpdateBookingDTO>>();
                if (apiResponse?.Data == null)
                {
                    return null;
                }

                var booking = apiResponse.Data;
                return new UpdateBookingVM
                {
                    BookingId = bookingId,
                    TableId = booking.TableId,
                    StartAt = booking.StartAt,
                    CustomerName = booking.CustomerName,
                    CustomerEmail = booking.CustomerEmail,
                    CustomerPhone = booking.CustomerPhone,
                    Amount = booking.Amount
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching booking {BookingId}: {Message}", bookingId, ex.Message);
                return null;
            }
        }

        private static List<BookingVM> GetFallbackBookings()
        {
            return new List<BookingVM>
            {
                new() {
                    CustomerName = "API not available",
                    CustomerEmail = "N/A",
                    StartAt = DateTime.Now
                }
            };
        }
    }
}