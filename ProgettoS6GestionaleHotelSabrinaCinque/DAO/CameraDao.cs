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
                        camere.Add(new Camera
                        {
                            Id = reader.GetInt32(0),
                            Descrizione = reader.GetString(1),
                            Tipologia = reader.GetString(2)
                        });
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
                                Id = reader.GetInt32(0),
                                Descrizione = reader.GetString(1),
                                Tipologia = reader.GetString(2)
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
                    INSERT INTO Camere (Descrizione, Tipologia)
                    VALUES (@Descrizione, @Tipologia)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descrizione", camera.Descrizione);
                    cmd.Parameters.AddWithValue("@Tipologia", camera.Tipologia);
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
                    Tipologia = @Tipologia
                    WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descrizione", camera.Descrizione);
                    cmd.Parameters.AddWithValue("@Tipologia", camera.Tipologia);
                    cmd.Parameters.AddWithValue("@Id", camera.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
