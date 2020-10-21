using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class Instituto
    {
        [Key]
        [DisplayName("Codigo")]
        public int InstitutoId { get; set; }
        public string Nombre { get; set; }
    }
}
