// Controllers/BookingController.cs - Uppdaterad version
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantMVC.DTOs;
using RestaurantMVC.Services;
using RestaurantMVC.ViewModels;

namespace RestaurantMVC.Controllers
{
    public class BookingController(
        IHttpClientFactory httpClientFactory,
        GetBookings getBookings,
        GetTables getTables) : Controller
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RestaurantAPI");
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ManageBookings()
        {
            var bookings = await getBookings.GetAllBookings();
            var tables = await getTables.GetAllTables();

            var viewModel = new ManageBookingsVM
            {
                Bookings = bookings,
                AllTables = tables
            };

            return View(viewModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditBooking(int id)
        {
            var booking = await getBookings.GetBookingForEdit(id);
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToAction("ManageBookings");
            }

            booking.AvailableTables = await getTables.GetAllTables();

            return View(booking);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditBooking(int id, UpdateBookingVM model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableTables = await getTables.GetAllTables();
                return View(model);
            }

            var updateDto = new UpdateBookingDTO
            {
                TableId = model.TableId,
                StartAt = model.StartAt,
                CustomerName = model.CustomerName,
                Amount = model.Amount
            };

            var response = await _httpClient.PutAsJsonAsync($"Booking/Updatebooking/{id}", updateDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Booking updated successfully!";
                return RedirectToAction("ManageBookings");
            }

            TempData["ErrorMessage"] = "Failed to update booking.";
            model.AvailableTables = await getTables.GetAllTables();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var response = await _httpClient.DeleteAsync($"Booking/DeleteBooking/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Booking deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete booking.";
            }

            return RedirectToAction("ManageBookings");
        }
    }
}