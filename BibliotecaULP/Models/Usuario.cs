using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Descripcion { get; set; }
        public string Clave { get; set; }
        public string Rol { get; set; }
        public String Imagen { get; set; }

        [NotMapped]
        public IFormFile ImagenFile { get; set; }
    }
}
