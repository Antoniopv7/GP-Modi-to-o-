using Gestion_Del_Presupuesto.Data;
using Gestion_Del_Presupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class ConvenioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConvenioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método Index que muestra la tabla de convenios con filtros opcionales
        public async Task<IActionResult> Index(string nombre, string tipo, string sede)
        {
            var conveniosQuery = _context.Convenios
                .Where(c => !c.Eliminado)
                .AsQueryable();

            // Aplicar filtros si se proporcionan
            if (!string.IsNullOrWhiteSpace(nombre))
                conveniosQuery = conveniosQuery.Where(c => c.Nombre.Contains(nombre));

            if (!string.IsNullOrWhiteSpace(tipo))
                conveniosQuery = conveniosQuery.Where(c => c.Tipo_Convenio.Equals(tipo, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(sede))
                conveniosQuery = conveniosQuery.Where(c => c.Sede.Equals(sede, StringComparison.OrdinalIgnoreCase));

            // Incluir relaciones necesarias y obtener datos para las vistas
            var convenios = await conveniosQuery
                .Include(c => c.Retribuciones)
                .Include(c => c.CentrosDeSalud)
                .ToListAsync();

            ViewBag.NombresConvenios = await _context.Convenios.Select(c => c.Nombre).Distinct().ToListAsync();
            ViewBag.TiposConvenios = await _context.Convenios.Select(c => c.Tipo_Convenio).Distinct().ToListAsync();
            ViewBag.Sedes = new List<string> { "Santiago", "Coquimbo", "Ambas" };

            return View(convenios);
        }

        // Método Create (GET)
        public IActionResult Create()
        {
            LoadViewData();
            return View();
        }

        // Método Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConvenioModel convenio)
        {
            if (!ModelState.IsValid)
            {
                LoadViewData();
                return View(convenio);
            }

            // Convertir las fechas a UTC
            convenio.Fecha_Inicio = DateTime.SpecifyKind(convenio.Fecha_Inicio, DateTimeKind.Utc);

            if (convenio.RenovacionAutomatica)
            {
                convenio.Fecha_Termino = DateTime.SpecifyKind(convenio.Fecha_Inicio.AddYears(1), DateTimeKind.Utc);
            }
            else if (convenio.Fecha_Termino.HasValue)
            {
                convenio.Fecha_Termino = DateTime.SpecifyKind(convenio.Fecha_Termino.Value, DateTimeKind.Utc);
            }

            // Asignar las relaciones adecuadamente (EF Core maneja esto automáticamente)
            if (convenio.Retribuciones != null && convenio.Retribuciones.Any())
            {
                foreach (var retribucion in convenio.Retribuciones)
                {
                    retribucion.FechaRetribucion = DateTime.SpecifyKind(retribucion.FechaRetribucion, DateTimeKind.Utc);
                }
            }

            if (convenio.CentrosDeSalud != null && convenio.CentrosDeSalud.Any())
            {
                foreach (var centro in convenio.CentrosDeSalud)
                {
                    // No es necesario asignar ConvenioId manualmente, EF lo hace automáticamente
                }
            }

            _context.Convenios.Add(convenio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convenio = await _context.Convenios
                .Include(c => c.CentrosDeSalud)
                .Include(c => c.Retribuciones)
                .FirstOrDefaultAsync(m => m.Id_Convenio == id);

            if (convenio == null)
            {
                return NotFound();
            }

            return View(convenio);
        }

        // Método Edit (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var convenio = await _context.Convenios
                .Include(c => c.Retribuciones)
                .Include(c => c.CentrosDeSalud)
                .FirstOrDefaultAsync(m => m.Id_Convenio == id);

            if (convenio == null) return NotFound();

            LoadViewData();

            return View(convenio);
        }

        // Método Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConvenioModel convenio)
        {
            if (id != convenio.Id_Convenio)
                return NotFound();

            if (!ModelState.IsValid)
            {
                LoadViewData();
                ViewBag.ErrorMessage = "El nombre y la sede son campos obligatorios.";
                return View(convenio);
            }

            // Convertir las fechas a UTC
            convenio.Fecha_Inicio = DateTime.SpecifyKind(convenio.Fecha_Inicio, DateTimeKind.Utc);

            if (convenio.RenovacionAutomatica)
            {
                convenio.Fecha_Termino = DateTime.SpecifyKind(convenio.Fecha_Inicio.AddYears(1), DateTimeKind.Utc);
            }
            else if (convenio.Fecha_Termino.HasValue)
            {
                convenio.Fecha_Termino = DateTime.SpecifyKind(convenio.Fecha_Termino.Value, DateTimeKind.Utc);
            }

            // Convertir las fechas de las retribuciones a UTC
            if (convenio.Retribuciones != null)
            {
                foreach (var retribucion in convenio.Retribuciones)
                {
                    retribucion.FechaRetribucion = DateTime.SpecifyKind(retribucion.FechaRetribucion, DateTimeKind.Utc);
                }
            }

            try
            {
                _context.Update(convenio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConvenioExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        // Método Delete
        public async Task<IActionResult> Delete(int id)
        {
            var convenio = await _context.Convenios.FindAsync(id);
            if (convenio != null)
            {
                convenio.Eliminado = true; // Marca el convenio como eliminado
                _context.Convenios.Update(convenio);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Método Restore
        public async Task<IActionResult> Restore(int id)
        {
            var convenio = await _context.Convenios.FindAsync(id);
            if (convenio != null)
            {
                convenio.Eliminado = false;
                _context.Convenios.Update(convenio);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Papelera));
        }

        // Método Papelera (Trash)
        public async Task<IActionResult> Papelera()
        {
            var convenios = await _context.Convenios.Where(c => c.Eliminado).ToListAsync();
            return View(convenios);
        }

        // Método DeletePermanent (Elimina permanentemente)
        [HttpGet]
        public async Task<IActionResult> DeletePermanent(int id)
        {
            var convenio = await _context.Convenios.FindAsync(id);
            if (convenio != null)
            {
                _context.Convenios.Remove(convenio); // Elimina el convenio permanentemente
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Papelera));
        }

        // Método para Exportar a Excel
        [HttpPost]
        public IActionResult ExportToExcel()
        {
            // Incluye las relaciones necesarias para obtener todos los datos
            var convenios = _context.Convenios
                .Include(c => c.Retribuciones)
                .Include(c => c.CentrosDeSalud)
                .Where(c => !c.Eliminado)
                .ToList();

            if (convenios == null || !convenios.Any())
            {
                TempData["ErrorMessage"] = "No hay convenios para exportar.";
                return RedirectToAction(nameof(Index));
            }

            // Configurar la licencia de EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Crear una hoja de cálculo
                var worksheet = package.Workbook.Worksheets.Add("Convenios");

                // Agregar los encabezados a la hoja de cálculo
                worksheet.Cells[1, 1].Value = "Id Convenio";
                worksheet.Cells[1, 2].Value = "Nombre";
                worksheet.Cells[1, 3].Value = "Tipo Convenio";
                worksheet.Cells[1, 4].Value = "Sede";
                worksheet.Cells[1, 5].Value = "Teléfono";
                worksheet.Cells[1, 6].Value = "Rut";
                worksheet.Cells[1, 7].Value = "Dirección";
                worksheet.Cells[1, 8].Value = "Contacto Principal";
                worksheet.Cells[1, 9].Value = "Fecha de Inicio";
                worksheet.Cells[1, 10].Value = "Fecha de Término";
                worksheet.Cells[1, 11].Value = "Renovación Automática";
                worksheet.Cells[1, 12].Value = "Tipo Retribución";
                worksheet.Cells[1, 13].Value = "Valor UF";
                worksheet.Cells[1, 14].Value = "Cantidad Pesos";
                worksheet.Cells[1, 15].Value = "Periodo";
                worksheet.Cells[1, 16].Value = "Tipo Práctica";
                worksheet.Cells[1, 17].Value = "Centro Salud - Nombre";
                worksheet.Cells[1, 18].Value = "Centro Salud - Dirección";
                worksheet.Cells[1, 19].Value = "Centro Salud - Contacto";
                worksheet.Cells[1, 20].Value = "Centro Salud - Correo";
                worksheet.Cells[1, 21].Value = "Centro Salud - Teléfono";

                int row = 2;

                // Iterar sobre los convenios y agregar los datos
                foreach (var convenio in convenios)
                {
                    int initialRow = row;

                    // Agregar datos del convenio
                    worksheet.Cells[row, 1].Value = convenio.Id_Convenio;
                    worksheet.Cells[row, 2].Value = convenio.Nombre;
                    worksheet.Cells[row, 3].Value = convenio.Tipo_Convenio;
                    worksheet.Cells[row, 4].Value = convenio.Sede;
                    worksheet.Cells[row, 5].Value = convenio.Telefono;
                    worksheet.Cells[row, 6].Value = convenio.Rut;
                    worksheet.Cells[row, 7].Value = convenio.Direccion;
                    worksheet.Cells[row, 8].Value = convenio.ContactoPrincipal;
                    worksheet.Cells[row, 9].Value = convenio.Fecha_Inicio.ToShortDateString();
                    worksheet.Cells[row, 10].Value = convenio.Fecha_Termino?.ToShortDateString();
                    worksheet.Cells[row, 11].Value = convenio.RenovacionAutomatica ? "Sí" : "No";

                    // Agregar las retribuciones
                    if (convenio.Retribuciones != null && convenio.Retribuciones.Any())
                    {
                        foreach (var retribucion in convenio.Retribuciones)
                        {
                            worksheet.Cells[row, 12].Value = retribucion.Tipo_Retribucion;
                            worksheet.Cells[row, 13].Value = retribucion.Monto;
                            worksheet.Cells[row, 14].Value = retribucion.CantPesos;
                            worksheet.Cells[row, 15].Value = retribucion.Periodo;
                            worksheet.Cells[row, 16].Value = retribucion.Tipo_Practica;
                            row++;
                        }
                    }
                    else
                    {
                        row++;
                    }

                    // Agregar los centros de salud
                    if (convenio.CentrosDeSalud != null && convenio.CentrosDeSalud.Any())
                    {
                        foreach (var centro in convenio.CentrosDeSalud)
                        {
                            worksheet.Cells[initialRow, 17].Value = centro.NombreCentro;
                            worksheet.Cells[initialRow, 18].Value = centro.Direccion;
                            worksheet.Cells[initialRow, 19].Value = centro.NombrecargocentroAso;
                            worksheet.Cells[initialRow, 20].Value = centro.CorreoPersonaCargo;
                            worksheet.Cells[initialRow, 21].Value = centro.Telefono_CentroAso;
                            initialRow++;
                        }
                    }
                }

                // Ajustar el ancho de las columnas automáticamente
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Guardar el archivo y devolverlo al usuario
                var stream = new MemoryStream();
                package.SaveAs(stream);
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Convenios.xlsx");
            }
        }

        // Método auxiliar para cargar datos en ViewBag
        private void LoadViewData()
        {
            ViewBag.Sedes = new List<string> { "Santiago", "Coquimbo", "Ambas" };
            ViewBag.TiposRetribucion = new List<string>
            {
                "Pago por Uso de CC $$",
                "Pago Apoyo a la Docencia",
                "Pago en RRHH",
                "Capacitación",
                "Obras Menores",
                "Obras Mayores",
                "Otros Gastos x retribución"
            };
        }

        // Método para verificar si un convenio existe
        private bool ConvenioExists(int id)
        {
            return _context.Convenios.Any(e => e.Id_Convenio == id);
        }
    }
}
