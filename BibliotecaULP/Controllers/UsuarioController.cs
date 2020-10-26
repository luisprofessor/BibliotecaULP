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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace BibliotecaULP.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration config;

        public UsuarioController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
        {
            this._context = contexto;
            this.config = config;
            this.environment = environment;
        }

        //[Authorize(Policy = "Administrador")]
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

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Usuario/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var user = _context.Usuario.FirstOrDefault(x => x.Email == loginView.Email);
                if (user == null || user.Clave != hashed)
                {
                    ViewBag.Mensaje = "Datos inválidos";
                    return View();
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("FullName", user.Nombre + " " + user.Apellido),
                    new Claim(ClaimTypes.Role, user.RolNombre),
                    new Claim("Id", user.UsuarioId + ""),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction(nameof(Index), "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [AllowAnonymous]
        [Route("salir", Name = "logout")]
        // GET: Home/Logout
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            var u = _context.Usuario.FirstOrDefault(x => x.Email == User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Perfil(Usuario user)
        {
            try
            {
                var usuario = _context.Usuario.FirstOrDefault(x => x.Email == User.Identity.Name);
                usuario.ImagenFile = user.ImagenFile;
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
                    _context.SaveChanges();
                }           
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.UsuarioId == id);
        }
    }
}
