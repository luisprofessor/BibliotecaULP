using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class RepositorioCarrera
    {
        private readonly string connectionString;

        private readonly IConfiguration configuration;
        public RepositorioCarrera(IConfiguration configuration)
        {
            this.configuration = configuration;

            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IList<Carrera> ObtenerTodos()
        {
            IList<Carrera> res = new List<Carrera>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT CarreraId, Nombre " +
                    "FROM Carrera";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    try
                    {
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Carrera c = new Carrera
                            {
                                CarreraId = reader.GetInt32(0),

                                Nombre = reader.GetString(1)

                            };

                            res.Add(c);
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










    }
}
