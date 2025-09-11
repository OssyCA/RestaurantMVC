using RestaurantMVC.Models;

namespace RestaurantMVC.ViewModels
{
    public class ManageTableVM
    {
        public TableVM NewTable { get; set; } = new TableVM();
        public List<RestaurantTable> Tables { get; set; } = [];
    }
}
