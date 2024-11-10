using Gestion_Del_Presupuesto.Models; // Incluir el espacio de nombres del modelo
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class RutificadoraController : Controller
    {
        // Lista estática para almacenar temporalmente los datos en memoria
        private static List<RutificadoraModel> rutificadoras = new List<RutificadoraModel>();

        // GET: Rutificadora/Index
        public IActionResult Index()
        {
            // Mostrar la lista completa de rutificadoras en la vista Index
            return View(rutificadoras);
        }

        // GET: Rutificadora/Details/5
        public IActionResult Details(int id)
        {
            // Buscar el registro por su Id en la lista
            var model = rutificadoras.FirstOrDefault(r => r.Id_Rutificadora == id);
            if (model == null)
            {
                return NotFound(); // Si no se encuentra, devuelve un error 404
            }
            return View(model);
        }

        // GET: Rutificadora/Create
        public IActionResult Create()
        {
            // Retorna la vista con un modelo vacío para crear un nuevo registro
            return View(new RutificadoraModel());
        }

        // POST: Rutificadora/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RutificadoraModel model)
        {
            if (ModelState.IsValid)
            {
                // Asignar un Id único al nuevo registro
                model.Id_Rutificadora = rutificadoras.Count + 1;
                rutificadoras.Add(model); // Agregar el nuevo registro a la lista
                return RedirectToAction(nameof(Index)); // Redirigir al listado
            }
            return View(model); // Si el modelo no es válido, volver a la vista con errores
        }

        // GET: Rutificadora/Edit/5
        public IActionResult Edit(int id)
        {
            // Buscar el registro existente por Id
            var model = rutificadoras.FirstOrDefault(r => r.Id_Rutificadora == id);
            if (model == null)
            {
                return NotFound(); // Si no se encuentra, devuelve un error 404
            }
            return View(model);
        }

        // POST: Rutificadora/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, RutificadoraModel model)
        {
            var existing = rutificadoras.FirstOrDefault(r => r.Id_Rutificadora == id); // Buscar el registro existente por Id
            if (existing == null)
            {
                return NotFound(); // Si no se encuentra, devuelve un error 404
            }

            if (ModelState.IsValid)
            {
                // Actualizar las propiedades del registro con los valores recibidos del formulario
                existing.Asignatura = model.Asignatura;
                existing.Institucion = model.Institucion;
                existing.NombreCompleto = model.NombreCompleto;
                existing.RUT = model.RUT;
                existing.FechaInicio = model.FechaInicio;
                existing.FechaTermino = model.FechaTermino;
                existing.NumeroDias = model.NumeroDias;
                existing.Observaciones = model.Observaciones;

                return RedirectToAction(nameof(Index)); // Redirigir al listado después de actualizar
            }
            return View(model); // Si el modelo no es válido, volver a la vista con errores
        }

        // GET: Rutificadora/Delete/5
        public IActionResult Delete(int id)
        {
            // Buscar el registro por su Id
            var model = rutificadoras.FirstOrDefault(r => r.Id_Rutificadora == id);
            if (model == null)
            {
                return NotFound(); // Si no se encuentra, devuelve un error 404
            }
            return View(model);
        }

        // POST: Rutificadora/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, RutificadoraModel model)
        {
            var existing = rutificadoras.FirstOrDefault(r => r.Id_Rutificadora == id); // Buscar el registro existente
            if (existing == null)
            {
                return NotFound(); // Si no se encuentra, devuelve un error 404
            }

            rutificadoras.Remove(existing); // Eliminar el registro de la lista
            return RedirectToAction(nameof(Index)); // Redirigir al listado
        }
    }
}
