using ProgettoS6GestionaleHotelSabrinaCinque.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT p.*, c.id AS cliente_id, c.cognome, c.nome, cam.id AS camera_id, cam.descrizione 
                    FROM Prenotazioni p
                    JOIN Clienti c ON p.cliente_id = c.id
                    JOIN Camere cam ON p.camera_id = cam.id";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var prenotazione = new Prenotazione
                            {
                                Id = (int)reader["id"],
                                DataPrenotazione = (DateTime)reader["data_prenotazione"],
                                NumeroProgressivo = (int)reader["numero_progressivo"],
                                Anno = (int)reader["anno"],
                                Dal = (DateTime)reader["dal"],
                                Al = (DateTime)reader["al"],
                                Caparra = (decimal)reader["caparra"],
                                Tariffa = (decimal)reader["tariffa"],
                                TipologiaSoggiorno = (string)reader["tipologia_soggiorno"],
                                ClienteId = (int)reader["cliente_id"],
                                CameraId = (int)reader["camera_id"],
                                Cliente = new Cliente
                                {
                                    Id = (int)reader["cliente_id"],
                                    Cognome = (string)reader["cognome"],
                                    Nome = (string)reader["nome"]
                                },
                                Camera = new Camera
                                {
                                    Id = (int)reader["camera_id"],
                                    Descrizione = (string)reader["descrizione"]
                                }
                            };
                            prenotazioni.Add(prenotazione);
                        }
                    }
                }
            }

            return prenotazioni;
        }

        public Prenotazione GetById(int id)
        {
            Prenotazione prenotazione = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT p.*, c.id AS cliente_id, c.cognome, c.nome, cam.id AS camera_id, cam.descrizione 
                    FROM Prenotazioni p
                    JOIN Clienti c ON p.cliente_id = c.id
                    JOIN Camere cam ON p.camera_id = cam.id
                    WHERE p.id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            prenotazione = new Prenotazione
                            {
                                Id = (int)reader["id"],
                                DataPrenotazione = (DateTime)reader["data_prenotazione"],
                                NumeroProgressivo = (int)reader["numero_progressivo"],
                                Anno = (int)reader["anno"],
                                Dal = (DateTime)reader["dal"],
                                Al = (DateTime)reader["al"],
                                Caparra = (decimal)reader["caparra"],
                                Tariffa = (decimal)reader["tariffa"],
                                TipologiaSoggiorno = (string)reader["tipologia_soggiorno"],
                                ClienteId = (int)reader["cliente_id"],
                                CameraId = (int)reader["camera_id"],
                                Cliente = new Cliente
                                {
                                    Id = (int)reader["cliente_id"],
                                    Cognome = (string)reader["cognome"],
                                    Nome = (string)reader["nome"]
                                },
                                Camera = new Camera
                                {
                                    Id = (int)reader["camera_id"],
                                    Descrizione = (string)reader["descrizione"]
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
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    INSERT INTO Prenotazioni (data_prenotazione, numero_progressivo, anno, dal, al, caparra, tariffa, tipologia_soggiorno, cliente_id, camera_id)
                    VALUES (@data_prenotazione, @numero_progressivo, @anno, @dal, @al, @caparra, @tariffa, @tipologia_soggiorno, @cliente_id, @camera_id)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@data_prenotazione", prenotazione.DataPrenotazione);
                    command.Parameters.AddWithValue("@numero_progressivo", prenotazione.NumeroProgressivo);
                    command.Parameters.AddWithValue("@anno", prenotazione.Anno);
                    command.Parameters.AddWithValue("@dal", prenotazione.Dal);
                    command.Parameters.AddWithValue("@al", prenotazione.Al);
                    command.Parameters.AddWithValue("@caparra", prenotazione.Caparra);
                    command.Parameters.AddWithValue("@tariffa", prenotazione.Tariffa);
                    command.Parameters.AddWithValue("@tipologia_soggiorno", prenotazione.TipologiaSoggiorno);
                    command.Parameters.AddWithValue("@cliente_id", prenotazione.ClienteId);
                    command.Parameters.AddWithValue("@camera_id", prenotazione.CameraId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Prenotazione prenotazione)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
            UPDATE Prenotazioni
            SET data_prenotazione = @data_prenotazione,
                numero_progressivo = @numero_progressivo,
                anno = @anno,
                dal = @dal,
                al = @al,
                caparra = @caparra,
                tariffa = @tariffa,
                tipologia_soggiorno = @tipologia_soggiorno,
                cliente_id = @cliente_id,
                camera_id = @camera_id
            WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", prenotazione.Id);
                    command.Parameters.AddWithValue("@data_prenotazione", prenotazione.DataPrenotazione);
                    command.Parameters.AddWithValue("@numero_progressivo", prenotazione.NumeroProgressivo);
                    command.Parameters.AddWithValue("@anno", prenotazione.Anno);
                    command.Parameters.AddWithValue("@dal", prenotazione.Dal);
                    command.Parameters.AddWithValue("@al", prenotazione.Al);
                    command.Parameters.AddWithValue("@caparra", prenotazione.Caparra);
                    command.Parameters.AddWithValue("@tariffa", prenotazione.Tariffa);
                    command.Parameters.AddWithValue("@tipologia_soggiorno", prenotazione.TipologiaSoggiorno);
                    command.Parameters.AddWithValue("@cliente_id", prenotazione.ClienteId);
                    command.Parameters.AddWithValue("@camera_id", prenotazione.CameraId);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Numero di righe aggiornate: {rowsAffected}");
                }
            }
        }


        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Prenotazioni WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
