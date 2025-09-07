namespace RestaurantMVC.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public bool IsPopular { get; set; } 
    }
}
