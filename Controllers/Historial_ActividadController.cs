using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gestion_Del_Presupuesto.Data;
using Gestion_Del_Presupuesto.Models;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class Historial_ActividadController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Historial_ActividadController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Historial_Actividad
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Historial_Actividad.Include(h => h.Usuarios);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Historial_Actividad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historial_Actividad = await _context.Historial_Actividad
                .Include(h => h.Usuarios)
                .FirstOrDefaultAsync(m => m.Id_Historial_Actividad == id);
            if (historial_Actividad == null)
            {
                return NotFound();
            }

            return View(historial_Actividad);
        }

        // GET: Historial_Actividad/Create
        public IActionResult Create()
        {
            ViewData["Id_Usuario"] = new SelectList(_context.Usuarios, "Id_Usuario", "Id_Usuario");
            return View();
        }

        // POST: Historial_Actividad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_Historial_Actividad,Fecha,Accion,Id_Usuario")] Historial_Actividad historial_Actividad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historial_Actividad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_Usuario"] = new SelectList(_context.Usuarios, "Id_Usuario", "Id_Usuario", historial_Actividad.Id_Usuario);
            return View(historial_Actividad);
        }

        // GET: Historial_Actividad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historial_Actividad = await _context.Historial_Actividad.FindAsync(id);
            if (historial_Actividad == null)
            {
                return NotFound();
            }
            ViewData["Id_Usuario"] = new SelectList(_context.Usuarios, "Id_Usuario", "Id_Usuario", historial_Actividad.Id_Usuario);
            return View(historial_Actividad);
        }

        // POST: Historial_Actividad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Historial_Actividad,Fecha,Accion,Id_Usuario")] Historial_Actividad historial_Actividad)
        {
            if (id != historial_Actividad.Id_Historial_Actividad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historial_Actividad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Historial_ActividadExists(historial_Actividad.Id_Historial_Actividad))
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
            ViewData["Id_Usuario"] = new SelectList(_context.Usuarios, "Id_Usuario", "Id_Usuario", historial_Actividad.Id_Usuario);
            return View(historial_Actividad);
        }

        // GET: Historial_Actividad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historial_Actividad = await _context.Historial_Actividad
                .Include(h => h.Usuarios)
                .FirstOrDefaultAsync(m => m.Id_Historial_Actividad == id);
            if (historial_Actividad == null)
            {
                return NotFound();
            }

            return View(historial_Actividad);
        }

        // POST: Historial_Actividad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historial_Actividad = await _context.Historial_Actividad.FindAsync(id);
            if (historial_Actividad != null)
            {
                _context.Historial_Actividad.Remove(historial_Actividad);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Historial_ActividadExists(int id)
        {
            return _context.Historial_Actividad.Any(e => e.Id_Historial_Actividad == id);
        }
    }
}
