using ProgettoS6GestionaleHotelSabrinaCinque.Models;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Services
{
    public interface IAuthService
    {
        User Login(string username, string password);
    }
}
