namespace RestaurantMVC.ViewModels
{
    public class TableVM
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool BookingLocked { get; set; }
    }
}
