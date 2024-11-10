using Gestion_Del_Presupuesto.Data; // Asegúrate de incluir la directiva de espacio de nombres
using Gestion_Del_Presupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class RetribucionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RetribucionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar todas las retribuciones
        public async Task<IActionResult> Index()
        {
            var retribuciones = await _context.Retribuciones.ToListAsync();
            return View(retribuciones);
        }

        // GET: Crear retribución
        public IActionResult Create()
        {
            return View();
        }

        // POST: Crear retribución
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RetribucionModel retribucion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(retribucion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(retribucion);
        }

        // GET: Editar retribución
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retribucion = await _context.Retribuciones.FindAsync(id);
            if (retribucion == null)
            {
                return NotFound();
            }
            return View(retribucion);
        }

        // POST: Editar retribución
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RetribucionModel retribucion)
        {
            if (id != retribucion.Id_Retribucion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(retribucion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetribucionExists(retribucion.Id_Retribucion))
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
            return View(retribucion);
        }

        // GET: Eliminar retribución
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retribucion = await _context.Retribuciones
                .FirstOrDefaultAsync(m => m.Id_Retribucion == id);
            if (retribucion == null)
            {
                return NotFound();
            }

            return View(retribucion);
        }

        // POST: Confirmar eliminación de retribución
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var retribucion = await _context.Retribuciones.FindAsync(id);
            _context.Retribuciones.Remove(retribucion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Verificar si existe una retribución
        private bool RetribucionExists(int id)
        {
            return _context.Retribuciones.Any(e => e.Id_Retribucion == id);
        }

        // Acción para seleccionar retribuciones y pasar a ViewBag
        public IActionResult SeleccionarRetribuciones()
        {
            var retribuciones = _context.Retribuciones.Select(r => new
            {
                Id_Retribucion = r.Id_Retribucion,
                TipoRetribucion = r.Tipo_Retribucion
            }).ToList();

            ViewBag.ProcesosRetribucion = retribuciones;

            return View();
        }
    }
}
