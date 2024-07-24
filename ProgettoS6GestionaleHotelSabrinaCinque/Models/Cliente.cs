namespace ProgettoS6GestionaleHotelSabrinaCinque.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string CodiceFiscale { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Citta { get; set; }
        public string Provincia { get; set; }
        public string Email { get; set; }
        public string Cellulare { get; set; }
    }
}
