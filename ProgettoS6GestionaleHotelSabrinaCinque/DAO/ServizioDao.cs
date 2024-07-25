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
                        var servizio = new Servizio
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Descrizione = reader.GetString(reader.GetOrdinal("descrizione")),
                            Prezzo = reader.GetDecimal(reader.GetOrdinal("prezzo"))
                        };

                        servizi.Add(servizio);
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
                var query = "SELECT * FROM Servizi WHERE id = @id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            servizio = new Servizio
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Descrizione = reader.GetString(reader.GetOrdinal("descrizione")),
                                Prezzo = reader.GetDecimal(reader.GetOrdinal("prezzo"))
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
                    INSERT INTO Servizi (descrizione, prezzo)
                    VALUES (@descrizione, @prezzo)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@descrizione", servizio.Descrizione);
                    cmd.Parameters.AddWithValue("@prezzo", servizio.Prezzo);
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
                    descrizione = @descrizione, 
                    prezzo = @prezzo
                    WHERE id = @id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@descrizione", servizio.Descrizione);
                    cmd.Parameters.AddWithValue("@prezzo", servizio.Prezzo);
                    cmd.Parameters.AddWithValue("@id", servizio.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "DELETE FROM Servizi WHERE id = @id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Servizio> GetByPrenotazioneId(int prenotazioneId)
        {
            var servizi = new List<Servizio>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"
                    SELECT s.*
                    FROM Servizi s
                    JOIN Prenotazioni_Servizi ps ON s.id = ps.servizio_id
                    WHERE ps.prenotazione_id = @prenotazione_id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@prenotazione_id", prenotazioneId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var servizio = new Servizio
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Descrizione = reader.GetString(reader.GetOrdinal("descrizione")),
                                Prezzo = reader.GetDecimal(reader.GetOrdinal("prezzo"))
                            };

                            servizi.Add(servizio);
                        }
                    }
                }
            }

            return servizi;
        }
    }
}
