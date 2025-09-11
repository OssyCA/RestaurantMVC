namespace RestaurantMVC.DTOs
{
    public class CreateTableDTO
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool BookingLocked { get; set; } = false; 
    }
}
