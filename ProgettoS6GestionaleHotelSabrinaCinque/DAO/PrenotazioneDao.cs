using System.Collections.Generic;
using System.Data.SqlClient;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;

namespace ProgettoS6GestionaleHotelSabrinaCinque.DAO
{
    public class PrenotazioneDao : IPrenotazioneDao
    {
        private readonly string _connectionString;

        public PrenotazioneDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Prenotazione> GetAll()
        {
            var prenotazioni = new List<Prenotazione>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Prenotazioni";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prenotazioni.Add(new Prenotazione
                        {
                            Id = reader.GetInt32(0),
                            ClienteId = reader.GetInt32(1),
                            CameraId = reader.GetInt32(2),
                            DataPrenotazione = reader.GetDateTime(3),
                            NumeroProgressivo = reader.GetInt32(4),
                            Anno = reader.GetInt32(5),
                            Dal = reader.GetDateTime(6),
                            Al = reader.GetDateTime(7),
                            Caparra = reader.GetDecimal(8),
                            Tariffa = reader.GetDecimal(9),
                            TipologiaSoggiorno = reader.GetString(10)
                        });
                    }
                }
            }

            return prenotazioni;
        }

        public Prenotazione GetById(int id)
        {
            Prenotazione prenotazione = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Prenotazioni WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            prenotazione = new Prenotazione
                            {
                                Id = reader.GetInt32(0),
                                ClienteId = reader.GetInt32(1),
                                CameraId = reader.GetInt32(2),
                                DataPrenotazione = reader.GetDateTime(3),
                                NumeroProgressivo = reader.GetInt32(4),
                                Anno = reader.GetInt32(5),
                                Dal = reader.GetDateTime(6),
                                Al = reader.GetDateTime(7),
                                Caparra = reader.GetDecimal(8),
                                Tariffa = reader.GetDecimal(9),
                                TipologiaSoggiorno = reader.GetString(10)
                            };
                        }
                    }
                }
            }

            return prenotazione;
        }

        public void Add(Prenotazione prenotazione)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"
                    INSERT INTO Prenotazioni (ClienteId, CameraId, DataPrenotazione, NumeroProgressivo, Anno, Dal, Al, Caparra, Tariffa, TipologiaSoggiorno)
                    VALUES (@ClienteId, @CameraId, @DataPrenotazione, @NumeroProgressivo, @Anno, @Dal, @Al, @Caparra, @Tariffa, @TipologiaSoggiorno)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClienteId", prenotazione.ClienteId);
                    cmd.Parameters.AddWithValue("@CameraId", prenotazione.CameraId);
                    cmd.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                    cmd.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                    cmd.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                    cmd.Parameters.AddWithValue("@Dal", prenotazione.Dal);
                    cmd.Parameters.AddWithValue("@Al", prenotazione.Al);
                    cmd.Parameters.AddWithValue("@Caparra", prenotazione.Caparra);
                    cmd.Parameters.AddWithValue("@Tariffa", prenotazione.Tariffa);
                    cmd.Parameters.AddWithValue("@TipologiaSoggiorno", prenotazione.TipologiaSoggiorno);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Prenotazione prenotazione)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"
                    UPDATE Prenotazioni SET 
                    ClienteId = @ClienteId,
                    CameraId = @CameraId,
                    DataPrenotazione = @DataPrenotazione,
                    NumeroProgressivo = @NumeroProgressivo,
                    Anno = @Anno,
                    Dal = @Dal,
                    Al = @Al,
                    Caparra = @Caparra,
                    Tariffa = @Tariffa,
                    TipologiaSoggiorno = @TipologiaSoggiorno
                    WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClienteId", prenotazione.ClienteId);
                    cmd.Parameters.AddWithValue("@CameraId", prenotazione.CameraId);
                    cmd.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                    cmd.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                    cmd.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                    cmd.Parameters.AddWithValue("@Dal", prenotazione.Dal);
                    cmd.Parameters.AddWithValue("@Al", prenotazione.Al);
                    cmd.Parameters.AddWithValue("@Caparra", prenotazione.Caparra);
                    cmd.Parameters.AddWithValue("@Tariffa", prenotazione.Tariffa);
                    cmd.Parameters.AddWithValue("@TipologiaSoggiorno", prenotazione.TipologiaSoggiorno);
                    cmd.Parameters.AddWithValue("@Id", prenotazione.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
