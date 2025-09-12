using RestaurantMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantMVC.ViewModels
{
    public class BookingVM
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantTableId { get; set; }
        public DateTime BookedAt { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? ExpireAt { get; set; }
        public int Amount { get; set; }
    }

}
