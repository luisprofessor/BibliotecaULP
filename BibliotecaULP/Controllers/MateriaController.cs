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
    public class MateriaController : Controller
    {
        private readonly IConfiguration configuration;

        private readonly RepositorioMateria repositorioMateria;
        public MateriaController(IConfiguration configuration)
        {
            this.configuration = configuration;

            repositorioMateria = new RepositorioMateria(configuration);
        }
        // GET: Materia
        public ActionResult Index()
        {

            try
            {
                var Materias = repositorioMateria.ObtenerTodos();

                return View(Materias);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // GET: Materia/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Materia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Materia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Materia/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Materia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Materia/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Materia/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}