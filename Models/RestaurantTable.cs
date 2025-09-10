using System.ComponentModel.DataAnnotations;

namespace RestaurantMVC.Models
{
    public class RestaurantTable
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool BookingLocked { get; set; }

    }
}
