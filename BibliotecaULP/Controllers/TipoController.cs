using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibliotecaULP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BibliotecaULP.Controllers
{
    public class TipoController : Controller
    {
        private readonly IConfiguration configuration;

        private readonly RepositorioTipo repositorioTipo;

        public TipoController(IConfiguration configuration)
        {
            this.configuration = configuration;

            repositorioTipo = new RepositorioTipo(configuration);
        }


        // GET: Tipo
        public ActionResult Index()
        {

            try
            {
                var tipos = repositorioTipo.ObtenerTodos();

                return View(tipos);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // GET: Tipo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tipo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tipo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tipo t)
        {
            try
            {
                repositorioTipo.Alta(t);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Tipo/Edit/5
        public ActionResult Edit(int id)
        {
            Tipo t = repositorioTipo.ObtenerPorId(id);

            return View(t);
        }

        // POST: Tipo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tipo t)
        {
            try
            {
                repositorioTipo.ModificarTipo(t);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Tipo/Delete/5
        public ActionResult Delete(int id)
        {

            var TipoABorrar = repositorioTipo.ObtenerPorId(id);

            return View(TipoABorrar);
        }

        // POST: Tipo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tipo t)
        {
            try
            {
                repositorioTipo.Baja(id);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}