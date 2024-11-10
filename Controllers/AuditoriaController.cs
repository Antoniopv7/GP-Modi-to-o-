using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gestion_Del_Presupuesto.Models;
using System.Collections.Generic;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class AuditoriaController : Controller
    {
        // GET: AuditoriaController/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: AuditoriaController/Historial
        public ActionResult Historial()
        {
            // Aquí deberías obtener el modelo real desde la base de datos.
            // Como ejemplo, se crea una lista de actividades con datos ficticios.
            var historial = new List<Historial_Actividad>
            {
                new Historial_Actividad { Usuarios = new Usuario { Nombre = "Admin" }, Accion = "Creación de registro", Fecha = System.DateTime.Now },
                new Historial_Actividad { Usuarios = new Usuario { Nombre = "User1" }, Accion = "Modificación de registro", Fecha = System.DateTime.Now.AddDays(-1) },
                new Historial_Actividad { Usuarios = new Usuario { Nombre = "User2" }, Accion = "Eliminación de registro", Fecha = System.DateTime.Now.AddDays(-2) }
            };

            // Pasar el modelo a la vista Historial.cshtml
            return View(historial);
        }

        // GET: AuditoriaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuditoriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuditoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: AuditoriaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuditoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: AuditoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuditoriaController/Delete/5
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
