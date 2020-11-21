using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class Tema
    {
        [Key]
        [DisplayName("Codigo")]
        public int TemaId { get; set; }
        public string Nombre { get; set; }
        public int MateriaId { get; set; }
        [ForeignKey("MateriaId")]
        public Materia Materia { get; set; }
    }
}
