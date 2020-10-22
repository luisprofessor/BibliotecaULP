using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BibliotecaULP.Models;
using Microsoft.Extensions.Configuration;

namespace BibliotecaULP.Controllers
{
    public class TemaController : Controller
    {
        private readonly DataContext _context;
        private readonly IConfiguration config;

        public TemaController(DataContext contexto, IConfiguration config)
        {
            this._context = contexto;
            this.config = config;
        }

        // GET: Tema
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tema.ToListAsync());
        }

        // GET: Tema/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tema = await _context.Tema
                .FirstOrDefaultAsync(m => m.TemaId == id);
            if (tema == null)
            {
                return NotFound();
            }

            return View(tema);
        }

        // GET: Tema/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tema/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TemaId,Nombre")] Tema tema)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tema);
        }

        // GET: Tema/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tema = await _context.Tema.FindAsync(id);
            if (tema == null)
            {
                return NotFound();
            }
            return View(tema);
        }

        // POST: Tema/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TemaId,Nombre")] Tema tema)
        {
            if (id != tema.TemaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemaExists(tema.TemaId))
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
            return View(tema);
        }

        // GET: Tema/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tema = await _context.Tema
                .FirstOrDefaultAsync(m => m.TemaId == id);
            if (tema == null)
            {
                return NotFound();
            }

            return View(tema);
        }

        // POST: Tema/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tema = await _context.Tema.FindAsync(id);
            _context.Tema.Remove(tema);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemaExists(int id)
        {
            return _context.Tema.Any(e => e.TemaId == id);
        }
    }
}
