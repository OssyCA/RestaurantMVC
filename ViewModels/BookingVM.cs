using RestaurantMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantMVC.ViewModels
{
    public class BookingVM
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? ExpireAt { get; set; }
        public int Amount { get; set; }

        public string FormattedStartTime => StartAt.ToString("yyyy-MM-dd HH:mm");
        public string FormattedExpireTime => ExpireAt?.ToString("yyyy-MM-dd HH:mm") ?? "N/A";

    }

}
