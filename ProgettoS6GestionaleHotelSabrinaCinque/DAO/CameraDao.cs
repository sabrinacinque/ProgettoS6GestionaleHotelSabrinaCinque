using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;

namespace ProgettoS6GestionaleHotelSabrinaCinque.DAO
{
    public class CameraDao : ICameraDao
    {
        private readonly string _connectionString;

        public CameraDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Camera> GetAll()
        {
            var camere = new List<Camera>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Camere";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var camera = new Camera
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                            Tipologia = reader.GetString(reader.GetOrdinal("Tipologia")),
                            Prezzo = reader.IsDBNull(reader.GetOrdinal("Prezzo")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Prezzo"))
                        };

                        camere.Add(camera);
                    }
                }
            }

            return camere;
        }

        public Camera GetById(int id)
        {
            Camera camera = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Camere WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            camera = new Camera
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                                Tipologia = reader.GetString(reader.GetOrdinal("Tipologia")),
                                Prezzo = reader.IsDBNull(reader.GetOrdinal("Prezzo")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Prezzo"))
                            };
                        }
                    }
                }
            }

            return camera;
        }

        public void Add(Camera camera)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"
                    INSERT INTO Camere (Descrizione, Tipologia, Prezzo)
                    VALUES (@Descrizione, @Tipologia, @Prezzo)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descrizione", camera.Descrizione);
                    cmd.Parameters.AddWithValue("@Tipologia", camera.Tipologia);
                    cmd.Parameters.AddWithValue("@Prezzo", camera.Prezzo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Camera camera)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"
                    UPDATE Camere SET 
                    Descrizione = @Descrizione, 
                    Tipologia = @Tipologia,
                    Prezzo = @Prezzo
                    WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descrizione", camera.Descrizione);
                    cmd.Parameters.AddWithValue("@Tipologia", camera.Tipologia);
                    cmd.Parameters.AddWithValue("@Prezzo", camera.Prezzo);
                    cmd.Parameters.AddWithValue("@Id", camera.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "DELETE FROM Camere WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
