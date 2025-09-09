using RestaurantMVC.Models;

namespace RestaurantMVC.Services.Iservices
{
    public interface IGetMenu
    {
        Task<List<MenuItem>> Menu();
    }
}
