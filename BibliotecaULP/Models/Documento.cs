using Microsoft.AspNetCore.Http;
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
        [DisplayName("Autor")]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
        public int TipoId { get; set; }
        [ForeignKey("TipoId")]
        public Tipo Tipo { get; set; }
        public int TemaId { get; set; }
        [ForeignKey("TemaId")]
        public Tema Tema { get; set; }
        public int MateriaId { get; set; }
        [ForeignKey("MateriaId")]
        public Materia Materia { get; set; }
        public DateTime FechaSubida { get; set; }
        public string DireccionDocumento{ get; set; }
        [NotMapped]
        public IFormFile Archivo { get; set; }

    }
}
