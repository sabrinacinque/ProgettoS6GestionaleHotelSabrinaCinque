namespace ProgettoS6GestionaleHotelSabrinaCinque.Models
{
    public class Prenotazione
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int CameraId { get; set; }
        public DateTime DataPrenotazione { get; set; }
        public int NumeroProgressivo { get; set; }
        public int Anno { get; set; }
        public DateTime Dal { get; set; }
        public DateTime Al { get; set; }
        public decimal Caparra { get; set; }
        public decimal Tariffa { get; set; }
        public string TipologiaSoggiorno { get; set; }

        public Cliente Cliente { get; set; }
        public Camera Camera { get; set; }
        //public List<Servizio> Servizi { get; set; } = new List<Servizio>();
    }
}
