using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaULP.Controllers
{
    public class Class
    {
    }
}


/*private readonly DataContext _context;
private readonly IHostingEnvironment environment;
private readonly IConfiguration config;

public DocumentoController(DataContext context, IConfiguration config, IHostingEnvironment environment)
{
    _context = context;
    this.config = config;
    this.environment = environment;
}

// GET: Documento
public async Task<IActionResult> Index()
{
    var dataContext = _context.Documento.Include(d => d.Usuario);

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

        if (documento.DocumentoId > 0 && documento.DireccionDocumento != null)
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

            documento.DireccionDocumento = "Uploads/Documentos" + fileName;

            _context.Update(documento);
        }

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "Apellido", documento.UsuarioId);
    return View(documento);
}*/