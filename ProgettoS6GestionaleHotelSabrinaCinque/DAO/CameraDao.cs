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
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Descrizione = reader.GetString(reader.GetOrdinal("descrizione")),
                            Tipologia = reader.GetString(reader.GetOrdinal("tipologia"))
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
                var query = "SELECT * FROM Camere WHERE id = @id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            camera = new Camera
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Descrizione = reader.GetString(reader.GetOrdinal("descrizione")),
                                Tipologia = reader.GetString(reader.GetOrdinal("tipologia"))
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
                    INSERT INTO Camere (descrizione, tipologia)
                    VALUES (@descrizione, @tipologia)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@descrizione", camera.Descrizione);
                    cmd.Parameters.AddWithValue("@tipologia", camera.Tipologia);
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
                    descrizione = @descrizione, 
                    tipologia = @tipologia
                    WHERE id = @id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@descrizione", camera.Descrizione);
                    cmd.Parameters.AddWithValue("@tipologia", camera.Tipologia);
                    cmd.Parameters.AddWithValue("@id", camera.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "DELETE FROM Camere WHERE id = @id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
