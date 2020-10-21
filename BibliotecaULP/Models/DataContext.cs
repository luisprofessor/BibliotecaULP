using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Materia> Materia { get; set; }
        public DbSet<Tema> Tema { get; set; }
        public DbSet<Tipo> Tipo { get; set; }
        public DbSet<Carrera> Carrera { get; set; }
        public DbSet<Instituto> Instituto { get; set; }
        public DbSet<Documento> Documento { get; set; }
    }
}
