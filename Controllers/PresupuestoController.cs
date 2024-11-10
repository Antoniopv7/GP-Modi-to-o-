using Gestion_Del_Presupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class PresupuestoController : Controller
    {
        private static List<PresupuestoModel> presupuestos = new List<PresupuestoModel>();

        // Lista de sedes y carreras por sede
        private static Dictionary<string, List<string>> carrerasPorSede = new Dictionary<string, List<string>>()
        {
            { "Santiago", new List<string> { "Enfermería", "Tecnología Médica", "Terapia Ocupacional", "Nutricion y Dietetica", "Psicologia", "Kinesiologia" } },
            { "La Serena", new List<string> { "Enfermeria","Terapia Ocupacional","Nutricion y Dietetica","Psicologia","Kinesiologia","Obstetricia y Puericultura" } }
        };

        private static List<string> formasRetribucion = new List<string>
        {
            "Pago por Uso de CC $$",
            "Pago Apoyo a la Docencia",
            "Pago en RRHH  ",
            "Capacitacion ",
            "Obras Menores",
            "Obras Mayores",
            "Otros Gastos x retribución"
        };

        public IActionResult Index(string carrera, string sede, int? year)
        {
            var registros = presupuestos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sede))
                registros = registros.Where(p => p.Sede == sede);

            if (!string.IsNullOrWhiteSpace(carrera))
                registros = registros.Where(p => p.Carrera.Contains(carrera));

            if (year.HasValue)
                registros = registros.Where(p => p.Anio == year.Value);

            ViewData["CarreraActual"] = carrera ?? string.Empty;
            ViewData["SedeActual"] = sede ?? string.Empty;
            ViewData["AnioActual"] = year;
            ViewBag.CarrerasPorSede = carrerasPorSede;

            return View(registros.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.CarrerasPorSede = carrerasPorSede;
            ViewBag.FormasRetribucion = formasRetribucion;
            return View(new PresupuestoModel { Anio = System.DateTime.Now.Year });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PresupuestoModel model, List<string> FormasRetribucionSeleccionadas)
        {
            if (ModelState.IsValid)
            {
                model.Id_PresupuestoXCentroCosto = presupuestos.Count + 1;
                model.FormasRetribucionSeleccionadas = FormasRetribucionSeleccionadas;
                model.TotalGastoEstimado = model.CostoMM + model.RRHHRetribucion + model.CapacitacionRetribucion + model.PagoApoyoDocencia + model.OtrosGastosRetribucion;
                presupuestos.Add(model);
                return RedirectToAction(nameof(Index), new { year = model.Anio });
            }
            ViewBag.CarrerasPorSede = carrerasPorSede;
            ViewBag.FormasRetribucion = formasRetribucion;
            return View(model);
        }
    }
}
