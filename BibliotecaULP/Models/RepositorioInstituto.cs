using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class RepositorioInstituto
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public RepositorioInstituto(IConfiguration configuration)
        {
            this.configuration = configuration;

            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IList<Instituto> ObtenerTodos()
        {
            IList<Instituto> res = new List<Instituto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT InstitutoId, Nombre " +
                    "FROM Instituto";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    try
                    {
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Instituto p = new Instituto
                            {
                                InstitutoId = reader.GetInt32(0),
                                
                                Nombre = reader.GetString(1)

                            };

                            res.Add(p);
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


        public int Alta(Instituto i)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Instituto (Nombre) " +
                    $"VALUES (@nombre);" +

                    $"SELECT SCOPE_IDENTITY();";//devuelve el id insertado

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("@nombre", i.Nombre);

                    connection.Open();

                    res = Convert.ToInt32(command.ExecuteScalar());

                    i.InstitutoId = res;

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
                string sql = $"DELETE FROM Instituto WHERE InstitutoId = {id}";

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


        public Instituto ObtenerPorId(int id)
        {
            Instituto res = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT InstitutoId, Nombre FROM Instituto" +
                    $" WHERE InstitutoId = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    command.CommandType = CommandType.Text;

                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res = new Instituto
                        {
                            InstitutoId = reader.GetInt32(0),

                            Nombre = reader.GetString(1)
                        };
                      

                    }
                    connection.Close();
                }
            }
            return res;
        }


        public int ModificarInstituto(Instituto i)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Instituto SET Nombre = @nombre " +
                    "WHERE InstitutoId = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", i.Nombre);

                    command.Parameters.AddWithValue("@id", i.InstitutoId);

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
