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

                                CarreraId = reader.GetInt32(1),

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


        public int Alta(Materia entidad)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sql = $"INSERT INTO Materia (MateriaId,ProfesorId,CarreraId,Nombre) " +
                    "VALUES (@materiaId, @profesorId, @carreraId, @nombre);" +
                    "SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@materiaId", entidad.MateriaId);

                    command.Parameters.AddWithValue("@profesorId", entidad.ProfesorId);

                    command.Parameters.AddWithValue("@carreraId", entidad.CarreraId);

                    command.Parameters.AddWithValue("@nombre", entidad.Nombre); 

                    connection.Open();

                    res = Convert.ToInt32(command.ExecuteScalar());

                    entidad.MateriaId = res;

                    connection.Close();
                }
            }
            return res;
        }

        public Materia ObtenerPorId(int id)
        {
            Materia res = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT m.MateriaId,m.CarreraId,c.Nombre,m.ProfesorId,u.Nombre,u.Apellido,m.Nombre" +
                    " FROM Materia m INNER JOIN Usuario u ON m.ProfesorId = u.UsuarioId" +
                    " INNER JOIN Carrera c ON m.CarreraId = c.CarreraId" +
                    " WHERE m.MateriaId = @id ";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    command.CommandType = CommandType.Text;

                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            res = new Materia
                            {
                                MateriaId = reader.GetInt32(0),

                                CarreraId = reader.GetInt32(1),

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
