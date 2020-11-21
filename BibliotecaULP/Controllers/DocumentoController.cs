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
using System.Dynamic;

namespace BibliotecaULP.Controllers
{
    //[Authorize]
    public class DocumentoController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration config;

        public DocumentoController(DataContext context,IConfiguration config, IWebHostEnvironment environment)
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
        
        public async Task<IActionResult> Create([Bind("DocumentoId,Nombre,UsuarioId,TipoId,TemaId,MateriaId,FechaSubida,Archivo")] Documento documento)
        {
            //Startup.Progress = 0;
            //long totalBytes = documento.Archivo.Length;

            if (ModelState.IsValid)
            {
                documento.FechaSubida = DateTime.Now;
                _context.Add(documento);
                _context.SaveChanges();

                if (documento.DocumentoId > 0 && documento.DireccionDocumento == null)
                {
                    string wwwpath = environment.WebRootPath;
                    string path = Path.Combine(wwwpath, "Uploads/Documentos");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = documento.Materia.Nombre + documento.Materia.Carrera.Nombre + documento.DocumentoId + Path.GetExtension(documento.Archivo.FileName);
                    string pathCompleto = Path.Combine(path, fileName);

                    //byte[] buffer = new byte[16 * 1024];
                    documento.DireccionDocumento = Path.Combine("/Uploads/Documentos/", fileName);

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                       documento.Archivo.CopyTo(stream);
                    }

                    /*using (FileStream output = System.IO.File.Create(pathCompleto))
                    {
                        using (Stream input = documento.Archivo.OpenReadStream())
                        {
                            long totalReadBytes = 0;
                            int readBytes;

                            while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                await output.WriteAsync(buffer, 0, readBytes);
                                totalReadBytes += readBytes;
                                Startup.Progress = (int)((float)totalReadBytes / (float)totalBytes * 100.0);
                                await Task.Delay(10);
                            }
                        }
                    }*/
                    _context.Update(documento);
                }

                _context.SaveChanges();

                //return this.Content("success");

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

        // GET: Documento/Search
        public IActionResult Search()
        {
         //   ViewData["MateriaId"] = new SelectList(_context.Materia, "materiaId", "Nombre");
           // ViewData["CarreraId"] = new SelectList(_context.Carrera, "CarreraId", "Nombre");
            //ViewData["InstitutoId"] = new SelectList(_context.Instituto, "InstitutoId", "Nombre");
            ViewBag.Tipos = _context.Tipo.ToList();
            return View();
        }

        // POST: Documento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("Nombre,MateriaId,CarreraId,InstitutoId")] SearchView search)
        {
           
            return View(search);
        }

        private bool DocumentoExists(int id)
        {
            return _context.Documento.Any(e => e.DocumentoId == id);
        }



        [HttpPost]
        public ActionResult Progress()
        {
            return this.Content(Startup.Progress.ToString());
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            string path = this.environment.WebRootPath + "\\uploads\\documentos";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path + filename;
        }
    }
}
