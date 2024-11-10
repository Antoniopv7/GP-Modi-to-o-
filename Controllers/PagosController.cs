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
    public class PagosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pagos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pagos.Include(p => p.Convenios);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Convenios)
                .FirstOrDefaultAsync(m => m.Id_Pago == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagos/Create
        public IActionResult Create()
        {
            ViewData["Id_Convenio"] = new SelectList(_context.Retribuciones, "Id_Retribucion", "Id_Retribucion");
            return View();
        }

        // POST: Pagos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_Pago,Fecha,FechaVencimiento,Monto,Nombre,Estado,Id_Retribucion,Id_Convenio")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_Convenio"] = new SelectList(_context.Retribuciones, "Id_Retribucion", "Id_Retribucion", pago.Id_Convenio);
            return View(pago);
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["Id_Convenio"] = new SelectList(_context.Retribuciones, "Id_Retribucion", "Id_Retribucion", pago.Id_Convenio);
            return View(pago);
        }

        // POST: Pagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Pago,Fecha,FechaVencimiento,Monto,Nombre,Estado,Id_Retribucion,Id_Convenio")] Pago pago)
        {
            if (id != pago.Id_Pago)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id_Pago))
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
            ViewData["Id_Convenio"] = new SelectList(_context.Retribuciones, "Id_Retribucion", "Id_Retribucion", pago.Id_Convenio);
            return View(pago);
        }

        // GET: Pagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Convenios)
                .FirstOrDefaultAsync(m => m.Id_Pago == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id_Pago == id);
        }
    }
}
