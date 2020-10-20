using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class RepositorioTipo
    {
        private readonly string connectionString;

        private readonly IConfiguration configuration;

        public RepositorioTipo(IConfiguration configuration)
        {
            this.configuration = configuration;

            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }


        public IList<Tipo> ObtenerTodos()
        {
            IList<Tipo> res = new List<Tipo>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT TipoId, Nombre " +
                    "FROM Tipo";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    try
                    {
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Tipo t = new Tipo
                            {
                                TipoId = reader.GetInt32(0),

                                Nombre = reader.GetString(1)

                            };

                            res.Add(t);
                        }

                    }
                    catch (System.Exception ex)
                    {


                    }

                    connection.Close();
                }
            }
            return res;
        }


        public int Alta(Tipo t)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Tipo (Nombre) " +
                    $"VALUES (@nombre);" +

                    $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("@nombre", t.Nombre);

                    connection.Open();

                    res = Convert.ToInt32(command.ExecuteScalar());

                    t.TipoId = res;

                    connection.Close();
                }
            }
            return res;
        }

        public Tipo ObtenerPorId(int id)
        {
            Tipo res = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT TipoId, Nombre FROM Tipo" +
                    $" WHERE TipoId = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    command.CommandType = CommandType.Text;

                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res = new Tipo
                        {
                            TipoId = reader.GetInt32(0),

                            Nombre = reader.GetString(1)
                        };


                    }
                    connection.Close();
                }
            }
            return res;
        }


        public int Baja(int id)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Tipo WHERE TipoId = {id}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    res = command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            return res;
        }

        public int ModificarTipo(Tipo t)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Tipo SET Nombre = @nombre " +
                    "WHERE TipoId = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", t.Nombre);

                    command.Parameters.AddWithValue("@id", t.TipoId);

                    command.CommandType = CommandType.Text;

                    connection.Open();

                    res = command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            return res;
        }



    }
}
