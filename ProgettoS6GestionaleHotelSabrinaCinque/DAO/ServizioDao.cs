using System.Collections.Generic;
using System.Data.SqlClient;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;

namespace ProgettoS6GestionaleHotelSabrinaCinque.DAO
{
    public class ServizioDao : IServizioDao
    {
        private readonly string _connectionString;

        public ServizioDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Servizio> GetAll()
        {
            var servizi = new List<Servizio>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Servizi";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        servizi.Add(new Servizio
                        {
                            Id = reader.GetInt32(0),
                            Descrizione = reader.GetString(1),
                            Prezzo = reader.GetDecimal(2)
                        });
                    }
                }
            }

            return servizi;
        }

        public Servizio GetById(int id)
        {
            Servizio servizio = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Servizi WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            servizio = new Servizio
                            {
                                Id = reader.GetInt32(0),
                                Descrizione = reader.GetString(1),
                                Prezzo = reader.GetDecimal(2)
                            };
                        }
                    }
                }
            }

            return servizio;
        }

        public void Add(Servizio servizio)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"
                    INSERT INTO Servizi (Descrizione, Prezzo)
                    VALUES (@Descrizione, @Prezzo)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descrizione", servizio.Descrizione);
                    cmd.Parameters.AddWithValue("@Prezzo", servizio.Prezzo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Servizio servizio)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"
                    UPDATE Servizi SET 
                    Descrizione = @Descrizione,
                    Prezzo = @Prezzo
                    WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descrizione", servizio.Descrizione);
                    cmd.Parameters.AddWithValue("@Prezzo", servizio.Prezzo);
                    cmd.Parameters.AddWithValue("@Id", servizio.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
