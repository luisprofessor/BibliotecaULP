using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BibliotecaULP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace BibliotecaULP.Controllers
{
    [Authorize]
    public class MateriaController : Controller
    {

        private readonly DataContext _context;

        private readonly IConfiguration config;


        public MateriaController(DataContext contexto, IConfiguration config)
        {
            this._context = contexto;

            this.config = config;
        }

        // GET: Materia
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Materia.Include(m => m.Carrera).Include(m => m.Profesor);

            return View(await dataContext.ToListAsync());
        }

        // GET: Materia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materia

                .Include(m => m.Carrera)

                .Include(m => m.Profesor)

                .FirstOrDefaultAsync(m => m.MateriaId == id);

            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // GET: Materia/Create
        public IActionResult Create()
        {
            ViewData["CarreraId"] = new SelectList(_context.Carrera, "CarreraId","Nombre");

            ViewData["ProfesorId"] = new SelectList(_context.Usuario.Where(x => x.Rol == 3), "UsuarioId", "Apellido");

            return View();
        }

        // POST: Materia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MateriaId,CarreraId,ProfesorId,Nombre")] Materia materia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materia);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CarreraId"] = new SelectList(_context.Carrera, "CarreraId", "Nombre", materia.CarreraId);

            ViewData["ProfesorId"] = new SelectList(_context.Usuario.Where(x => x.Rol == 3), "UsuarioId", "Apellido", materia.ProfesorId);

            return View(materia);
        }

        // GET: Materia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materia.FindAsync(id);

            if (materia == null)
            {
                return NotFound();
            }
            ViewData["CarreraId"] = new SelectList(_context.Carrera, "CarreraId", "Nombre", materia.CarreraId );

            ViewData["ProfesorId"] = new SelectList(_context.Usuario.Where(x => x.Rol == 3), "UsuarioId", "Apellido", materia.ProfesorId);

            return View(materia);
        }

        // POST: Materia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MateriaId,CarreraId,ProfesorId,Nombre")] Materia materia)
        {
            if (id != materia.MateriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materia);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaExists(materia.MateriaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarreraId"] = new SelectList(_context.Carrera, "CarreraId", "CarreraId", materia.CarreraId);

            ViewData["ProfesorId"] = new SelectList(_context.Usuario, "UsuarioId", "Clave", materia.ProfesorId);

            return View(materia);
        }

        // GET: Materia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materia
                .Include(m => m.Carrera)

                .Include(m => m.Profesor)

                .FirstOrDefaultAsync(m => m.MateriaId == id);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // POST: Materia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materia = await _context.Materia.FindAsync(id);

            _context.Materia.Remove(materia);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MateriaExists(int id)
        {
            return _context.Materia.Any(e => e.MateriaId == id);
        }
    }
}
