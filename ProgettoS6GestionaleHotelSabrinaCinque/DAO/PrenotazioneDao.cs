using System;
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
                var query = @"
                    SELECT p.*, c.cognome, c.nome, cam.descrizione 
                    FROM Prenotazioni p
                    JOIN Clienti c ON p.cliente_id = c.id
                    JOIN Camere cam ON p.camera_id = cam.id";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var prenotazione = new Prenotazione
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            ClienteId = reader.GetInt32(reader.GetOrdinal("cliente_id")),
                            CameraId = reader.GetInt32(reader.GetOrdinal("camera_id")),
                            DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("data_prenotazione")),
                            NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("numero_progressivo")),
                            Anno = reader.GetInt32(reader.GetOrdinal("anno")),
                            Dal = reader.GetDateTime(reader.GetOrdinal("dal")),
                            Al = reader.GetDateTime(reader.GetOrdinal("al")),
                            Caparra = reader.GetDecimal(reader.GetOrdinal("caparra")),
                            Tariffa = reader.GetDecimal(reader.GetOrdinal("tariffa")),
                            TipologiaSoggiorno = reader.GetString(reader.GetOrdinal("tipologia_soggiorno")),
                            Cliente = new Cliente
                            {
                                Cognome = reader.GetString(reader.GetOrdinal("cognome")),
                                Nome = reader.GetString(reader.GetOrdinal("nome"))
                            },
                            Camera = new Camera
                            {
                                Descrizione = reader.GetString(reader.GetOrdinal("descrizione"))
                            }
                        };
                        prenotazioni.Add(prenotazione);
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
                var query = @"
                    SELECT p.*, c.cognome, c.nome, cam.descrizione 
                    FROM Prenotazioni p
                    JOIN Clienti c ON p.cliente_id = c.id
                    JOIN Camere cam ON p.camera_id = cam.id
                    WHERE p.id = @id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            prenotazione = new Prenotazione
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                ClienteId = reader.GetInt32(reader.GetOrdinal("cliente_id")),
                                CameraId = reader.GetInt32(reader.GetOrdinal("camera_id")),
                                DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("data_prenotazione")),
                                NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("numero_progressivo")),
                                Anno = reader.GetInt32(reader.GetOrdinal("anno")),
                                Dal = reader.GetDateTime(reader.GetOrdinal("dal")),
                                Al = reader.GetDateTime(reader.GetOrdinal("al")),
                                Caparra = reader.GetDecimal(reader.GetOrdinal("caparra")),
                                Tariffa = reader.GetDecimal(reader.GetOrdinal("tariffa")),
                                TipologiaSoggiorno = reader.GetString(reader.GetOrdinal("tipologia_soggiorno")),
                                Cliente = new Cliente
                                {
                                    Cognome = reader.GetString(reader.GetOrdinal("cognome")),
                                    Nome = reader.GetString(reader.GetOrdinal("nome"))
                                },
                                Camera = new Camera
                                {
                                    Descrizione = reader.GetString(reader.GetOrdinal("descrizione"))
                                }
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
                    INSERT INTO Prenotazioni (cliente_id, camera_id, data_prenotazione, numero_progressivo, anno, dal, al, caparra, tariffa, tipologia_soggiorno)
                    VALUES (@cliente_id, @camera_id, @data_prenotazione, @numero_progressivo, @anno, @dal, @al, @caparra, @tariffa, @tipologia_soggiorno);
                    SELECT SCOPE_IDENTITY();";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cliente_id", prenotazione.ClienteId);
                    cmd.Parameters.AddWithValue("@camera_id", prenotazione.CameraId);
                    cmd.Parameters.AddWithValue("@data_prenotazione", prenotazione.DataPrenotazione);
                    cmd.Parameters.AddWithValue("@numero_progressivo", prenotazione.NumeroProgressivo);
                    cmd.Parameters.AddWithValue("@anno", prenotazione.Anno);
                    cmd.Parameters.AddWithValue("@dal", prenotazione.Dal);
                    cmd.Parameters.AddWithValue("@al", prenotazione.Al);
                    cmd.Parameters.AddWithValue("@caparra", prenotazione.Caparra);
                    cmd.Parameters.AddWithValue("@tariffa", prenotazione.Tariffa);
                    cmd.Parameters.AddWithValue("@tipologia_soggiorno", prenotazione.TipologiaSoggiorno);
                    prenotazione.Id = Convert.ToInt32(cmd.ExecuteScalar());
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
                    cliente_id = @cliente_id, 
                    camera_id = @camera_id, 
                    data_prenotazione = @data_prenotazione, 
                    numero_progressivo = @numero_progressivo, 
                    anno = @anno, 
                    dal = @dal, 
                    al = @al, 
                    caparra = @caparra, 
                    tariffa = @tariffa, 
                    tipologia_soggiorno = @tipologia_soggiorno
                    WHERE id = @id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cliente_id", prenotazione.ClienteId);
                    cmd.Parameters.AddWithValue("@camera_id", prenotazione.CameraId);
                    cmd.Parameters.AddWithValue("@data_prenotazione", prenotazione.DataPrenotazione);
                    cmd.Parameters.AddWithValue("@numero_progressivo", prenotazione.NumeroProgressivo);
                    cmd.Parameters.AddWithValue("@anno", prenotazione.Anno);
                    cmd.Parameters.AddWithValue("@dal", prenotazione.Dal);
                    cmd.Parameters.AddWithValue("@al", prenotazione.Al);
                    cmd.Parameters.AddWithValue("@caparra", prenotazione.Caparra);
                    cmd.Parameters.AddWithValue("@tariffa", prenotazione.Tariffa);
                    cmd.Parameters.AddWithValue("@tipologia_soggiorno", prenotazione.TipologiaSoggiorno);
                    cmd.Parameters.AddWithValue("@id", prenotazione.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var deleteQuery = "DELETE FROM Prenotazioni WHERE id = @id";
                using (var cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
