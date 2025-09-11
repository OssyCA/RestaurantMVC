namespace RestaurantMVC.DTOs
{
    public class UpdateTableDTO
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool BookingLocked { get; set; } = false;
    }
}
