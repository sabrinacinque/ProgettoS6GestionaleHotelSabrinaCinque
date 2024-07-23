using System.Collections.Generic;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;

namespace ProgettoS6GestionaleHotelSabrinaCinque.DAO
{
    public interface ICameraDao
    {
        IEnumerable<Camera> GetAll();
        Camera GetById(int id);
        void Add(Camera camera);
        void Update(Camera camera);
        void Delete(int id);
    }
}
