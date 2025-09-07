using RestaurantMVC.Models;

namespace RestaurantMVC.Services.IGetMenuService
{
    public interface IGetMenu
    {
        Task<List<MenuItem>> Menu();
    }
}
