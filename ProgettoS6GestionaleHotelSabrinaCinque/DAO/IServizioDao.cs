using System.Collections.Generic;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;

namespace ProgettoS6GestionaleHotelSabrinaCinque.DAO
{
    public interface IServizioDao
    {
        IEnumerable<Servizio> GetAll();
        Servizio GetById(int id);
        void Add(Servizio servizio);
        void Update(Servizio servizio);
        void Delete(int id);
        IEnumerable<Servizio> GetByPrenotazioneId(int prenotazioneId);
    }
}
