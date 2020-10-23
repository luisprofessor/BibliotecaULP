using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaULP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BibliotecaULP.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly DataContext _context;
        private readonly IHostingEnvironment environment;
        private readonly IConfiguration config;

        public UsuarioController(DataContext contexto, IConfiguration config, IHostingEnvironment environment)
        {
            this._context = contexto;
            this.config = config;
            this.environment = environment;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nombre,Apellido,Descripcion,Email,Clave,Rol,ImagenFile")] Usuario usuario)
        {
            try
            {                          
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    usuario.Clave = hashed;
                    usuario.Imagen = "";
                    usuario.Rol = User.IsInRole("Administrador") || User.IsInRole("SuperAdministrador") ? usuario.Rol : (int)enRoles.Profesor;
                    var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                    _context.Add(usuario);
                    _context.SaveChanges();
                    usuario.UsuarioId = _context.Usuario.AsNoTracking().LastOrDefault().UsuarioId;
                    TempData["Id"] = usuario.UsuarioId;
                    if (usuario.ImagenFile != null && usuario.UsuarioId > 0)
                    {
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "Uploads");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "avatar_" + usuario.UsuarioId + Path.GetExtension(usuario.ImagenFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        usuario.Imagen = Path.Combine("/Uploads", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            usuario.ImagenFile.CopyTo(stream);
                        }
                        _context.Update(usuario);
                    }
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                    return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(usuario);
            }
          
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nombre,Apellido,Descripcion,Clave,Rol,Imagen")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /*
        [HttpPost("login")]
        //[AllowAnonymous]
        public async Task<IActionResult> Login(LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = _context.Usuario.FirstOrDefault(x => x.Email == loginView.Email);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }

                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Email),
                        new Claim("FullName", p.Nombre + " " + p.Apellido),
                        new Claim(ClaimTypes.Role, "Usuario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        */
        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.UsuarioId == id);
        }
    }
}
