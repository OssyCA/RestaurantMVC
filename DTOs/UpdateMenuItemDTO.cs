namespace RestaurantMVC.DTOs
{
    internal class UpdateMenuItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPopular { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}