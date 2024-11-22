using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using Gestion_Del_Presupuesto.Data;
using Gestion_Del_Presupuesto.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CsvHelper;
using System.Globalization;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class PlanillasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanillasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Planillas
        public async Task<IActionResult> Index(string asignatura, string institucion, string carrera, string nombre, string rut)
        {
            var planillasQuery = _context.Planillas.Include(p => p.Estudiante).AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(asignatura))
            {
                planillasQuery = planillasQuery.Where(p => p.Asignatura.Contains(asignatura));
            }

            if (!string.IsNullOrWhiteSpace(institucion))
            {
                planillasQuery = planillasQuery.Where(p => p.Institución.Contains(institucion));
            }

            if (!string.IsNullOrWhiteSpace(carrera))
            {
                planillasQuery = planillasQuery.Where(p => p.Carrera.Nombre.Contains(carrera));
            }

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                planillasQuery = planillasQuery.Where(p => p.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrWhiteSpace(rut))
            {
                planillasQuery = planillasQuery.Where(p => p.Rut.ToString().Contains(rut));
            }

            var planillas = await planillasQuery.ToListAsync();
            return View(planillas);
        }


        // GET: Planillas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planilla = await _context.Planillas.Include(p => p.Estudiante).FirstOrDefaultAsync(m => m.Id_Planillas == id);
            if (planilla == null)
            {
                return NotFound();
            }

            return View(planilla);
        }

        // GET: Planillas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Planillas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_Planillas,Nombre_Planilla,Rut,Fecha_Inicio,Fecha_Termino,Institución,CuantasSemanas,ValorUfContrato,TotalCosto,Id_Estudiante")] PlanillasModel planilla)
        {
            if (ModelState.IsValid)
            {
                _context.Add(planilla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planilla);
        }

        // GET: Planillas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planilla = await _context.Planillas.Include(p => p.Estudiante).FirstOrDefaultAsync(p => p.Id_Planillas == id);
            if (planilla == null)
            {
                return NotFound();
            }
            return View(planilla);
        }

        // POST: Planillas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Planillas,Nombre_Planilla,Rut,Fecha_Inicio,Fecha_Termino,Institución,CuantasSemanas,ValorUfContrato,TotalCosto,Id_Estudiante")] PlanillasModel planilla)
        {
            if (id != planilla.Id_Planillas)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planilla);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanillaExists(planilla.Id_Planillas))
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
            return View(planilla);
        }

        // GET: Planillas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planilla = await _context.Planillas.Include(p => p.Estudiante).FirstOrDefaultAsync(m => m.Id_Planillas == id);
            if (planilla == null)
            {
                return NotFound();
            }

            return View(planilla);
        }

        // POST: Planillas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planilla = await _context.Planillas.FindAsync(id);
            _context.Planillas.Remove(planilla);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanillaExists(int id)
        {
            return _context.Planillas.Any(e => e.Id_Planillas == id);
        }

        // GET: Planillas/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Planillas/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Selecciona un archivo válido.";
                return View();
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Lógica para leer el archivo CSV y agregar los datos a la base de datos
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                try
                {
                    var records = csv.GetRecords<PlanillasModel>().ToList();
                    _context.Planillas.AddRange(records);
                    await _context.SaveChangesAsync();
                    ViewBag.Message = "Archivo subido y procesado correctamente.";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Error procesando el archivo: {ex.Message}";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
