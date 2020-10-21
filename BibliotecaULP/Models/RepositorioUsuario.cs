using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class RepositorioUsuario
    {
        private readonly string connectionString;

        private readonly IConfiguration configuration;
        public RepositorioUsuario(IConfiguration configuration)
        {
            this.configuration = configuration;

            connectionString = configuration["ConnectionStrings:DefaultConnection"];

        }

        public IList<Usuario> ObtenerTodosLosProfesores()
        {
            IList<Usuario> res = new List<Usuario>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT UsuarioId,Nombre,Apellido,Descripcion,Rol,Imagen" +
                    " FROM Usuario " +
                    " WHERE Rol = 'Profesor'";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Usuario p = new Usuario
                            {
                                UsuarioId = reader.GetInt32(0),                            

                                Nombre = reader.GetString(1),

                                Apellido = reader.GetString(2),

                                Descripcion = reader.GetString(3),

                                Rol = reader.GetString(4),

                                Imagen = reader.GetString(5)

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
