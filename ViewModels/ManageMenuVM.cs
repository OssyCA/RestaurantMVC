using RestaurantMVC.DTOs;
using RestaurantMVC.Models;

namespace RestaurantMVC.ViewModels
{
    public class ManageMenuVM
    {
        public MenuItemVM NewMenuItem { get; set; } = new();
        public List<MenuItem> MenuItems { get; set; } = [];
    }
}
