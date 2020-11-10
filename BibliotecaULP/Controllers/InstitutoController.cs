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
    //[Authorize]
    public class InstitutoController : Controller
    {
        private readonly DataContext _context;
        private readonly IConfiguration config;

        public InstitutoController(DataContext contexto, IConfiguration config)
        {
            this._context = contexto;
            this.config = config;
        }

        // GET: Instituto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Instituto.ToListAsync());
        }

        // GET: Instituto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituto = await _context.Instituto
                .FirstOrDefaultAsync(m => m.InstitutoId == id);
            if (instituto == null)
            {
                return NotFound();
            }

            return View(instituto);
        }

        // GET: Instituto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instituto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstitutoId,Nombre")] Instituto instituto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instituto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instituto);
        }

        // GET: Instituto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituto = await _context.Instituto.FindAsync(id);
            if (instituto == null)
            {
                return NotFound();
            }
            return View(instituto);
        }

        // POST: Instituto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstitutoId,Nombre")] Instituto instituto)
        {
            if (id != instituto.InstitutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instituto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutoExists(instituto.InstitutoId))
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
            return View(instituto);
        }

        // GET: Instituto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituto = await _context.Instituto
                .FirstOrDefaultAsync(m => m.InstitutoId == id);
            if (instituto == null)
            {
                return NotFound();
            }

            return View(instituto);
        }

        // POST: Instituto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instituto = await _context.Instituto.FindAsync(id);
            _context.Instituto.Remove(instituto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutoExists(int id)
        {
            return _context.Instituto.Any(e => e.InstitutoId == id);
        }
    }
}
