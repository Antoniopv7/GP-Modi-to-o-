using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion_Del_Presupuesto.Models;
using Gestion_Del_Presupuesto.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class DevengadoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DevengadoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Devengado
        public async Task<IActionResult> Index()
        {
            var devengados = await _context.Devengados.Include(d => d.Convenio).ToListAsync();
            return View(devengados);
        }

        // GET: Devengado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devengado = await _context.Devengados
                .Include(d => d.Convenio)
                .FirstOrDefaultAsync(m => m.Id_Devengado == id);

            if (devengado == null)
            {
                return NotFound();
            }

            return View(devengado);
        }

        // GET: Devengado/Create
        public IActionResult Create()
        {
            ViewData["ConvenioId"] = new SelectList(_context.Convenios, "Id", "Nombre");
            return View();
        }

        // POST: Devengado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_Devengado,Carrera,CentroCosto,Itempresupuestario,FechaInicio,FechaFin,CantidadTiempo,GastoTotalComprometidoMonto,CantEstudiantes,ValorUFDevengado,CostoUF,PagosRealizados,SaldoPendiente,Descripcion,TotalGastoDevengadoGeneradoporEstudiantes,ConvenioId")] Devengado devengado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(devengado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConvenioId"] = new SelectList(_context.Convenios, "Id", "Nombre", devengado.ConvenioId);
            return View(devengado);
        }

        // GET: Devengado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devengado = await _context.Devengados.FindAsync(id);
            if (devengado == null)
            {
                return NotFound();
            }
            ViewData["ConvenioId"] = new SelectList(_context.Convenios, "Id", "Nombre", devengado.ConvenioId);
            return View(devengado);
        }

        // POST: Devengado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Devengado,Carrera,CentroCosto,Itempresupuestario,FechaInicio,FechaFin,CantidadTiempo,GastoTotalComprometidoMonto,CantEstudiantes,ValorUFDevengado,CostoUF,PagosRealizados,SaldoPendiente,Descripcion,TotalGastoDevengadoGeneradoporEstudiantes,ConvenioId")] Devengado devengado)
        {
            if (id != devengado.Id_Devengado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devengado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevengadoExists(devengado.Id_Devengado))
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
            ViewData["ConvenioId"] = new SelectList(_context.Convenios, "Id", "Nombre", devengado.ConvenioId);
            return View(devengado);
        }

        // GET: Devengado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devengado = await _context.Devengados
                .Include(d => d.Convenio)
                .FirstOrDefaultAsync(m => m.Id_Devengado == id);
            if (devengado == null)
            {
                return NotFound();
            }

            return View(devengado);
        }

        // POST: Devengado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var devengado = await _context.Devengados.FindAsync(id);
            _context.Devengados.Remove(devengado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevengadoExists(int id)
        {
            return _context.Devengados.Any(e => e.Id_Devengado == id);
        }
        [HttpPost]
        public async Task<IActionResult> CalcularDevengado(int id)
        {
            // Recuperar la instancia de Devengado desde la base de datos
            var devengado = await _context.Devengados.FindAsync(id);
            if (devengado == null)
            {
                return NotFound("Devengado no encontrado");
            }

            // Llamar al método CalcularDevengado
            var totalDevengado = devengado.CalcularDevengado();

            // Actualizar el valor en la base de datos (si es necesario)
            devengado.TotalGastoDevengadoGeneradoporEstudiantes = totalDevengado;
            _context.Update(devengado);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
