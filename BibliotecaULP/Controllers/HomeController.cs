using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BibliotecaULP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using React;

namespace BibliotecaULP.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        private readonly IConfiguration config;
        public HomeController(DataContext contexto, IConfiguration config)
        {
            this._context = contexto;
            this.config = config;
        }
        public async Task<IActionResult> Index()
        {
            /*var dataContext = _context.Documento.Include(d => d.Materia).Include(d => d.Tema).Include(d => d.Tipo).Include(d => d.Usuario);
            return View(await dataContext.ToListAsync());*/
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
