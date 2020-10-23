using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BibliotecaULP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace BibliotecaULP.Controllers
{
    [Authorize]
    public class DocumentoController : Controller
    {
        private readonly DataContext _context;
        private readonly IHostingEnvironment environment;
        private readonly IConfiguration config;

        public DocumentoController(DataContext context,IConfiguration config, IHostingEnvironment environment)
        {
            _context = context;
            this.config = config;
            this.environment = environment;
        }

        // GET: Documento
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Documento.Include(d => d.Materia).Include(d => d.Tema).Include(d => d.Tipo).Include(d => d.Usuario);
            return View(await dataContext.ToListAsync());
        }

        // GET: Documento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documento = await _context.Documento
                .Include(d => d.Materia)
                .Include(d => d.Tema)
                .Include(d => d.Tipo)
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.DocumentoId == id);
            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        // GET: Documento/Create
        public IActionResult Create()
        {
            ViewData["MateriaId"] = new SelectList(_context.Materia, "MateriaId", "MateriaId");
            ViewData["TemaId"] = new SelectList(_context.Tema, "TemaId", "TemaId");
            ViewData["TipoId"] = new SelectList(_context.Tipo, "TipoId", "TipoId");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "Apellido");
            return View();
        }

        // POST: Documento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocumentoId,Nombre,UsuarioId,TipoId,TemaId,MateriaId,FechaSubida,Archivo")] Documento documento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(documento);

                await _context.SaveChangesAsync();

                if (documento.DocumentoId > 0 && documento.DireccionDocumento == null)
                {
                    string wwwpath = environment.WebRootPath;

                    string path = Path.Combine(wwwpath, "Uploads/Documentos");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fileName = documento.Nombre + Path.GetExtension(documento.Archivo.FileName);

                    string pathCompleto = Path.Combine(path, fileName);

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        documento.Archivo.CopyTo(stream);
                    }

                    documento.DireccionDocumento = "Uploads/Documentos/" + fileName + documento.DocumentoId;

                    _context.Update(documento);
                }

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewData["MateriaId"] = new SelectList(_context.Materia, "MateriaId", "MateriaId", documento.MateriaId);
            ViewData["TemaId"] = new SelectList(_context.Tema, "TemaId", "TemaId", documento.TemaId);
            ViewData["TipoId"] = new SelectList(_context.Tipo, "TipoId", "TipoId", documento.TipoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "Apellido", documento.UsuarioId);
            return View(documento);
        }

        // GET: Documento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documento = await _context.Documento.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }
            ViewData["MateriaId"] = new SelectList(_context.Materia, "MateriaId", "MateriaId", documento.MateriaId);
            ViewData["TemaId"] = new SelectList(_context.Tema, "TemaId", "TemaId", documento.TemaId);
            ViewData["TipoId"] = new SelectList(_context.Tipo, "TipoId", "TipoId", documento.TipoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "Clave", documento.UsuarioId);
            return View(documento);
        }

        // POST: Documento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocumentoId,Nombre,UsuarioId,TipoId,TemaId,MateriaId,FechaSubida,DireccionDocumento")] Documento documento)
        {
            if (id != documento.DocumentoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentoExists(documento.DocumentoId))
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
            ViewData["MateriaId"] = new SelectList(_context.Materia, "MateriaId", "MateriaId", documento.MateriaId);
            ViewData["TemaId"] = new SelectList(_context.Tema, "TemaId", "TemaId", documento.TemaId);
            ViewData["TipoId"] = new SelectList(_context.Tipo, "TipoId", "TipoId", documento.TipoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "Clave", documento.UsuarioId);
            return View(documento);
        }

        // GET: Documento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documento = await _context.Documento
                .Include(d => d.Materia)
                .Include(d => d.Tema)
                .Include(d => d.Tipo)
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.DocumentoId == id);
            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        // POST: Documento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var documento = await _context.Documento.FindAsync(id);

            string wwwpath = environment.WebRootPath;

            String PathCompleto = wwwpath + "/" + documento.DireccionDocumento;

            try
            {
                System.IO.File.Delete(PathCompleto);

                _context.Documento.Remove(documento);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return RedirectToAction(nameof(Index));
            }
           
        }

        private bool DocumentoExists(int id)
        {
            return _context.Documento.Any(e => e.DocumentoId == id);
        }
    }
}
