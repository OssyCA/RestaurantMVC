using System.ComponentModel.DataAnnotations;

namespace RestaurantMVC.ViewModels
{
    public class MenuItemVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Need title")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage ="Need a description")]
        public string Description { get; set; } = null!;
        public bool IsPopular { get; set; }
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? ImageUrl { get; set; }
        [Range(0.01, 1000.00, ErrorMessage = "Price must be between 0.01 and 1000.00")]
        public decimal Price { get; set; }
    }
}
