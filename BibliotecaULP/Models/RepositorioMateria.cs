using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class RepositorioMateria
    {
        private readonly string connectionString;

        private readonly IConfiguration configuration;
        public RepositorioMateria(IConfiguration configuration)
        {
            this.configuration = configuration;

            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }
        public IList<Materia> ObtenerTodos()
        {
            IList<Materia> res = new List<Materia>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT m.MateriaId,m.CarreraId,c.Nombre,m.ProfesorId,u.Nombre,u.Apellido,m.Nombre" +
                    " FROM Materia m INNER JOIN Usuario u ON m.ProfesorId = u.UsuarioId" +
                    " INNER JOIN Carrera c ON m.CarreraId = c.CarreraId";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Materia p = new Materia
                            {
                                MateriaId = reader.GetInt32(0),

                                CarrerId = reader.GetInt32(1),

                                Carrera = new Carrera
                                {
                                    CarreraId = reader.GetInt32(1),

                                    Nombre = reader.GetString(2),
                                },

                                ProfesorId = reader.GetInt32(3),

                                Profesor = new Usuario
                                {
                                    UsuarioId = reader.GetInt32(3),

                                    Nombre = reader.GetString(4),

                                    Apellido = reader.GetString(5),
                                },

                                Nombre = reader.GetString(6),

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

    }
}
