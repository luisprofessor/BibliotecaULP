using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class Carrera
    {
        [Key]
        public int CarreraId { get; set; }
        public int InstitutoId { get; set; }
        public string Nombre { get; set; }
    }
}
