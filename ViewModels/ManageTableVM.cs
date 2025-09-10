using RestaurantMVC.Models;

namespace RestaurantMVC.ViewModels
{
    public class ManageTableVM
    {
        public RestaurantTable NewTable { get; set; } = new();
        public List<RestaurantTable> Tables { get; set; } = new();
    }
}
