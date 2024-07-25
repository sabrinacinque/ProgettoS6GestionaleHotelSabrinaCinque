using System.ComponentModel.DataAnnotations;

namespace ProgettoS6GestionaleHotelSabrinaCinque.Models
{
    public class Prenotazione
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo Cliente è obbligatorio.")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Il campo Camera è obbligatorio.")]
        public int CameraId { get; set; }

        [Required(ErrorMessage = "Il campo Data Prenotazione è obbligatorio.")]
        public DateTime DataPrenotazione { get; set; }

        public int NumeroProgressivo { get; set; }
        public int Anno { get; set; }
        public DateTime Dal { get; set; }
        public DateTime Al { get; set; }
        public decimal Caparra { get; set; }
        public decimal Tariffa { get; set; }
        public string TipologiaSoggiorno { get; set; }

        public Cliente? Cliente { get; set; }
        public Camera? Camera { get; set; }

        public List<int> ServiziSelezionati { get; set; } = new List<int>();
        public List<Servizio>? Servizi { get; set; }
    }
}
