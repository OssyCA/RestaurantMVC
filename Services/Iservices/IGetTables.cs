using RestaurantMVC.Models;

namespace RestaurantMVC.Services.Iservices
{
    public interface IGetTables
    {
        Task<List<RestaurantTable>> GetAllTables();
    }
}
