using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class Documento
    {
        [Key]
        [DisplayName("Codigo")]
        public int DocumentoId { get; set; }
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public int TipoId { get; set; }
        [ForeignKey("TipoId")]
        public int TemaId { get; set; }
        [ForeignKey("TemaId")]
        public int MateriaId { get; set; }
        [ForeignKey("MateriaId")]
        public DateTime FechaSubida { get; set; }

    }
}
