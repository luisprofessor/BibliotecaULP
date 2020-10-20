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
    public class InstitutoController : Controller
    {
        private readonly IConfiguration configuration;

        private readonly RepositorioInstituto repoInstituto;
        public InstitutoController(IConfiguration configuration)
        {
            this.configuration = configuration;

            this.repoInstituto = new RepositorioInstituto(configuration);
        }
        // GET: Instituto
        public ActionResult Index()
        {
            var Institutos = repoInstituto.ObtenerTodos();

            return View(Institutos);
        }

        // GET: Instituto/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Instituto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instituto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Instituto i)
        {
            try
            {
                // TODO: Add insert logic here
                repoInstituto.Alta(i);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Instituto/Edit/5
        public ActionResult Edit(int id)
        {
            var InstitutoAEditar = repoInstituto.ObtenerPorId(id);

            return View(InstitutoAEditar);
        }

        // POST: Instituto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Instituto i)
        {
            try
            {
                // TODO: Add update logic here
                repoInstituto.ModificarInstituto(i);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Instituto/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                repoInstituto.Baja(id);

                return RedirectToAction(nameof(Index));

            }
            catch(Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
           
        }

        // POST: Instituto/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
               

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}