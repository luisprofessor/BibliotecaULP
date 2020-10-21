using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class Materia
    {
        [Key]
        public int MateriaId { get; set; }

        public int CarreraId { get; set; }

        [ForeignKey("CarrerId")]
        public Carrera Carrera { get; set; }

        public int ProfesorId { get; set; }

        [ForeignKey("ProfesorId")]
        public Usuario Profesor { get; set; }

        public string Nombre { get; set; }
    }
}
