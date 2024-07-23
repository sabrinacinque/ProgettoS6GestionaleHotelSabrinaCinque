using System.Collections.Generic;
using System.Data.SqlClient;
using ProgettoS6GestionaleHotelSabrinaCinque.Models;

namespace ProgettoS6GestionaleHotelSabrinaCinque.DAO
{
    public class ClienteDao : IClienteDao
    {
        private readonly string _connectionString;

        public ClienteDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Cliente> GetAll()
        {
            var clienti = new List<Cliente>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Clienti";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clienti.Add(new Cliente
                        {
                            Id = reader.GetInt32(0),
                            CodiceFiscale = reader.GetString(1),
                            Cognome = reader.GetString(2),
                            Nome = reader.GetString(3),
                            Citta = reader.GetString(4),
                            Provincia = reader.GetString(5),
                            Email = reader.GetString(6),
                            Cellulare = reader.GetString(7)
                        });
                    }
                }
            }

            return clienti;
        }

        public Cliente GetById(int id)
        {
            Cliente cliente = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT * FROM Clienti WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Cliente
                            {
                                Id = reader.GetInt32(0),
                                CodiceFiscale = reader.GetString(1),
                                Cognome = reader.GetString(2),
                                Nome = reader.GetString(3),
                                Citta = reader.GetString(4),
                                Provincia = reader.GetString(5),
                                Email = reader.GetString(6),
                                Cellulare = reader.GetString(7)
                            };
                        }
                    }
                }
            }

            return cliente;
        }

        public void Add(Cliente cliente)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"
                    INSERT INTO Clienti (CodiceFiscale, Cognome, Nome, Citta, Provincia, Email, Cellulare)
                    VALUES (@CodiceFiscale, @Cognome, @Nome, @Citta, @Provincia, @Email, @Cellulare)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                    cmd.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@Citta", cliente.Citta);
                    cmd.Parameters.AddWithValue("@Provincia", cliente.Provincia);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);
                    cmd.Parameters.AddWithValue("@Cellulare", cliente.Cellulare);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Cliente cliente)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"
                    UPDATE Clienti SET 
                    CodiceFiscale = @CodiceFiscale,
                    Cognome = @Cognome,
                    Nome = @Nome,
                    Citta = @Citta,
                    Provincia = @Provincia,
                    Email = @Email,
                    Cellulare = @Cellulare
                    WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                    cmd.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@Citta", cliente.Citta);
                    cmd.Parameters.AddWithValue("@Provincia", cliente.Provincia);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);
                    cmd.Parameters.AddWithValue("@Cellulare", cliente.Cellulare);
                    cmd.Parameters.AddWithValue("@Id", cliente.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
