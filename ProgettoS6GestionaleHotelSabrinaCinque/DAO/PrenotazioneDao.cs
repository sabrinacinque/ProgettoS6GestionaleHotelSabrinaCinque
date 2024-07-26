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
                    SELECT p.*, c.id AS cliente_id, c.cognome, c.nome, cam.id AS camera_id, cam.descrizione, cam.prezzo 
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
                                    Descrizione = (string)reader["descrizione"],
                                    Prezzo = (decimal)reader["prezzo"]
                                },
                                Servizi = new List<Servizio>()
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
                    SELECT p.*, c.id AS cliente_id, c.cognome, c.nome, cam.id AS camera_id, cam.descrizione, cam.prezzo 
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
                                    Descrizione = (string)reader["descrizione"],
                                    Prezzo = (decimal)reader["prezzo"]
                                },
                                Servizi = new List<Servizio>()
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
                    VALUES (@data_prenotazione, @numero_progressivo, @anno, @dal, @al, @caparra, @tariffa, @tipologia_soggiorno, @cliente_id, @camera_id);
                    SELECT SCOPE_IDENTITY();";
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
                    prenotazione.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                UpdateServizi(prenotazione.Id, prenotazione.ServiziSelezionati);
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
                    command.ExecuteNonQuery();
                }
                UpdateServizi(prenotazione.Id, prenotazione.ServiziSelezionati);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Prima eliminiamo i record correlati nella tabella Prenotazioni_Servizi,altrimenti mi dava conflitto con l'eliminazione della prenotazione e non me la faceva eliminare
                var deleteServiziQuery = "DELETE FROM Prenotazioni_Servizi WHERE prenotazione_id = @prenotazione_id";
                using (var cmd = new SqlCommand(deleteServiziQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@prenotazione_id", id);
                    cmd.ExecuteNonQuery();
                }

                // Poi eliminiamo la prenotazione
                var deleteQuery = "DELETE FROM Prenotazioni WHERE Id = @Id";
                using (var cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int GetLastId()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT ISNULL(MAX(Id), 0) FROM Prenotazioni";
                using (var cmd = new SqlCommand(query, conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateServizi(int prenotazioneId, List<int> serviziSelezionati)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var deleteQuery = "DELETE FROM Prenotazioni_Servizi WHERE prenotazione_id = @prenotazione_id";
                using (var cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@prenotazione_id", prenotazioneId);
                    cmd.ExecuteNonQuery();
                }

                if (serviziSelezionati != null && serviziSelezionati.Count > 0)
                {
                    var insertQuery = "INSERT INTO Prenotazioni_Servizi (prenotazione_id, servizio_id) VALUES (@prenotazione_id, @servizio_id)";
                    foreach (var servizioId in serviziSelezionati)
                    {
                        using (var cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@prenotazione_id", prenotazioneId);
                            cmd.Parameters.AddWithValue("@servizio_id", servizioId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public IEnumerable<Prenotazione> GetPrenotazioniByCodiceFiscale(string codiceFiscale)
        {
            var prenotazioni = new List<Prenotazione>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT p.*, c.cognome, c.nome, cam.descrizione 
                    FROM Prenotazioni p
                    JOIN Clienti c ON p.cliente_id = c.id
                    JOIN Camere cam ON p.camera_id = cam.id
                    WHERE c.codice_fiscale = @codiceFiscale";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@codiceFiscale", codiceFiscale);
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

        public int GetTotalePrenotazioniPerTipologia(string tipologiaSoggiorno)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT COUNT(*)
                    FROM Prenotazioni
                    WHERE tipologia_soggiorno = @tipologiaSoggiorno";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tipologiaSoggiorno", tipologiaSoggiorno);
                    return (int)command.ExecuteScalar();
                }
            }
        }

    }
}
