using RestaurantMVC.Models;

namespace RestaurantMVC.Services.Iservices
{
    public interface IGetMenu
    {
        void ClearMenuCache();
        Task<List<MenuItem>> Menu();
    }
}
