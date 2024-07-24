using System.Collections.Generic;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;

namespace ProgettoS6GestionaleHotelSabrinaCinque.DAO
{
    public interface IPrenotazioneDao
    {
        IEnumerable<Prenotazione> GetAll();
        Prenotazione GetById(int id);

        void Add(Prenotazione prenotazione);
        void Update(Prenotazione prenotazione);
        //void Add(Prenotazione prenotazione, List<int> serviziIds);
        //void Update(Prenotazione prenotazione, List<int> serviziIds);
        void Delete(int id);
    }
}
