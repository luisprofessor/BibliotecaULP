using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class Carrera
    {
        [Key]
        [DisplayName("Codigo")]
        public int CarreraId { get; set; }
        public int InstitutoId { get; set; }
        [ForeignKey("InstitutoId")]
        public Instituto Instituto { get; set; }
        public string Nombre { get; set; }
    }
}
