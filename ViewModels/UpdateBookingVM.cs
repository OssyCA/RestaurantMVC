using RestaurantMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantMVC.ViewModels
{
    public class UpdateBookingVM
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Table is required")]
        public int TableId { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [Display(Name = "Start Time")]
        public DateTime StartAt { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Number of guests is required")]
        [Range(1, 20, ErrorMessage = "Amount must be between 1 and 20")]
        [Display(Name = "Number of Guests")]
        public int Amount { get; set; }

        [Display(Name = "Customer Email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Display(Name = "Customer Phone")]
        [Phone(ErrorMessage = "Invalid phone format")]
        public string CustomerPhone { get; set; } = string.Empty;
        public List<RestaurantTable> AvailableTables { get; set; } = [];
    }
}