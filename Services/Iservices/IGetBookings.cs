using RestaurantMVC.ViewModels;

namespace RestaurantMVC.Services.Iservices
{
    public interface IGetBookings
    {
        Task<List<BookingVM>> GetAllBookings();
        Task<UpdateBookingVM?> GetBookingForEdit(int bookingId);
    }
}
