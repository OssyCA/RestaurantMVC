using RestaurantMVC.Models;

namespace RestaurantMVC.ViewModels
{
    public class ManageBookingsVM
    {
        public List<BookingVM> Bookings { get; set; } = [];
        public List<RestaurantTable> AllTables { get; set; } = [];
    }
}
