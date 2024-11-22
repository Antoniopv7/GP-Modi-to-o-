using Gestion_Del_Presupuesto.Data;
using Gestion_Del_Presupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
            var conveniosQuery = _context.Convenios.Where(c => !c.Eliminado).AsQueryable();

            // Aplicar filtros si se proporcionan
            if (!string.IsNullOrWhiteSpace(nombre))
                conveniosQuery = conveniosQuery.Where(c => c.Nombre.Contains(nombre));

            if (!string.IsNullOrWhiteSpace(tipo))
                conveniosQuery = conveniosQuery.Where(c => c.Tipo_Convenio.Equals(tipo, System.StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(sede))
                conveniosQuery = conveniosQuery.Where(c => c.Sede.Equals(sede, System.StringComparison.OrdinalIgnoreCase));

            // Incluir relaciones necesarias y obtener datos para las vistas
            var convenios = await conveniosQuery.Include(c => c.Retribuciones).ToListAsync();
            ViewBag.NombresConvenios = await _context.Convenios.Select(c => c.Nombre).Distinct().ToListAsync();
            ViewBag.TiposConvenios = await _context.Convenios.Select(c => c.Tipo_Convenio).Distinct().ToListAsync();
            ViewBag.Sedes = new List<string> { "Santiago", "Coquimbo", "Ambas Sedes" };

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
        public async Task<IActionResult> Create(ConvenioModel convenio, bool renovacionAutomatica)
        {
            if (!ModelState.IsValid)
            {
                LoadViewData();
                return View(convenio);
            }

            // Convertir las fechas a UTC
            convenio.Fecha_Inicio = DateTime.SpecifyKind(convenio.Fecha_Inicio, DateTimeKind.Utc);

            if (renovacionAutomatica)
            {
                convenio.Fecha_Termino = DateTime.SpecifyKind(convenio.Fecha_Inicio.AddYears(1), DateTimeKind.Utc);
            }
            else if (convenio.Fecha_Termino.HasValue)
            {
                convenio.Fecha_Termino = DateTime.SpecifyKind(convenio.Fecha_Termino.Value, DateTimeKind.Utc);
            }

            // Asignar las relaciones adecuadamente
            if (convenio.Retribuciones != null)
            {
                foreach (var retribucion in convenio.Retribuciones)
                {
                    retribucion.FechaRetribucion = DateTime.SpecifyKind(retribucion.FechaRetribucion, DateTimeKind.Utc);
                }
            }

            _context.Add(convenio);
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

            // Buscar el convenio en la base de datos
            var convenio = await _context.Convenios
                .Include(c => c.Retribuciones)
                .Include(c => c.CentrosDeSalud)
                .FirstOrDefaultAsync(m => m.Id_Convenio == id);

            if (convenio == null) return NotFound();
            ViewBag.TiposConvenio = _context.Convenios.Select(tc => tc.Nombre).ToList();

            LoadViewData();

            return View(convenio);
        }

        // Método Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConvenioModel convenio, bool renovacionAutomatica, bool adendum)
        {
            if (id != convenio.Id_Convenio)
                return NotFound();

            if (!ModelState.IsValid)
            {
                LoadViewData();
                return View(convenio);
            }

            var convenioExistente = await _context.Convenios
                .Include(c => c.Retribuciones)
                .Include(c => c.CentrosDeSalud)
                .FirstOrDefaultAsync(c => c.Id_Convenio == id);

            if (convenioExistente == null)
                return NotFound();

            // Crear nueva versión si el adendum está activado y se modifica la fecha o la observación del adendum
            if (adendum &&
                (convenioExistente.FechaAdendum != convenio.FechaAdendum ||
                 convenioExistente.ObservacionAdendum != convenio.ObservacionAdendum))
            {
                var nuevoConvenio = new ConvenioModel
                {
                    Nombre = convenioExistente.Nombre,
                    Tipo_Convenio = convenioExistente.Tipo_Convenio,
                    Sede = convenioExistente.Sede,
                    Rut = convenioExistente.Rut,
                    Direccion = convenioExistente.Direccion,
                    ContactoPrincipal = convenioExistente.ContactoPrincipal,
                    Telefono = convenioExistente.Telefono,
                    Fecha_Inicio = DateTime.SpecifyKind(convenio.Fecha_Inicio, DateTimeKind.Utc),
                    Fecha_Termino = convenio.Fecha_Termino,
                    RenovacionAutomatica = renovacionAutomatica,
                    Adendum = adendum,
                    FechaAdendum = convenio.FechaAdendum,
                    ObservacionAdendum = convenio.ObservacionAdendum,
                    ValorUF = convenioExistente.ValorUF,
                    Eliminado = convenioExistente.Eliminado,
                    Version = convenioExistente.Version + 1, // Incrementar versión
                    Retribuciones = convenioExistente.Retribuciones.Select(r => new RetribucionModel
                    {
                        Tipo_Retribucion = r.Tipo_Retribucion,
                        Monto = r.Monto,
                        CantPesos = r.CantPesos,
                        Periodo = r.Periodo,
                        Tipo_Practica = r.Tipo_Practica,
                        FechaRetribucion = r.FechaRetribucion
                    }).ToList(),
                    CentrosDeSalud = convenioExistente.CentrosDeSalud.Select(cs => new CentroSaludModel
                    {
                        NombreCentro = cs.NombreCentro,
                        Direccion = cs.Direccion,
                        NombrecargocentroAso = cs.NombrecargocentroAso,
                        CorreoPersonaCargo = cs.CorreoPersonaCargo,
                        Telefono_CentroAso = cs.Telefono_CentroAso
                    }).ToList(),
                    Devengados = convenioExistente.Devengados.ToList(),
                    Facturacion = convenioExistente.Facturacion.ToList()
                };

                // Agregar el nuevo convenio a la base de datos
                _context.Convenios.Add(nuevoConvenio);
            }
            else
            {
                // Limpia listas relacionadas antes de asignar los nuevos valores
                convenioExistente.Retribuciones.Clear();
                convenioExistente.CentrosDeSalud.Clear();

                // Actualiza las propiedades principales del convenio existente
                convenioExistente.Nombre = convenio.Nombre;
                convenioExistente.Tipo_Convenio = convenio.Tipo_Convenio;
                convenioExistente.Sede = convenio.Sede;
                convenioExistente.Rut = convenio.Rut;
                convenioExistente.Direccion = convenio.Direccion;
                convenioExistente.ContactoPrincipal = convenio.ContactoPrincipal;
                convenioExistente.Telefono = convenio.Telefono;
                convenioExistente.Fecha_Inicio = DateTime.SpecifyKind(convenio.Fecha_Inicio, DateTimeKind.Utc);

                if (renovacionAutomatica)
                    convenioExistente.Fecha_Termino = DateTime.SpecifyKind(convenio.Fecha_Inicio.AddYears(1), DateTimeKind.Utc);
                else if (convenio.Fecha_Termino.HasValue)
                    convenioExistente.Fecha_Termino = DateTime.SpecifyKind(convenio.Fecha_Termino.Value, DateTimeKind.Utc);

                convenioExistente.RenovacionAutomatica = renovacionAutomatica;
                convenioExistente.Adendum = adendum;
                convenioExistente.FechaAdendum = convenio.FechaAdendum;
                convenioExistente.ObservacionAdendum = convenio.ObservacionAdendum;

                // Reasignar relaciones
                if (convenio.Retribuciones != null)
                {
                    foreach (var retribucion in convenio.Retribuciones)
                    {
                        retribucion.FechaRetribucion = DateTime.SpecifyKind(retribucion.FechaRetribucion, DateTimeKind.Utc);
                        convenioExistente.Retribuciones.Add(retribucion);
                    }
                }

                if (convenio.CentrosDeSalud != null)
                {
                    foreach (var centro in convenio.CentrosDeSalud)
                    {
                        convenioExistente.CentrosDeSalud.Add(centro);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConvenioExists(id))
                    return NotFound();
                throw;
            }
        }

        public async Task<IActionResult> Papelera()
        {
            var convenios = await _context.Convenios.Where(c => c.Eliminado).ToListAsync();
            return View(convenios);
        }


        // Método Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id, IFormFile documentoTermino)
        {
            // Verificar si el archivo se ha subido
            if (documentoTermino == null || documentoTermino.Length == 0)
            {
                TempData["ErrorMessage"] = "Debe proporcionar un documento de término para proceder con la eliminación.";
                return RedirectToAction(nameof(Index));
            }

            // Guardar el documento de término en el servidor o base de datos si es necesario
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\Users\\olahl\\Source\\Repos\\Campos-Clinicos-Sin-Errores-de-Compilacion\\Documentos_Termino\\", documentoTermino.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await documentoTermino.CopyToAsync(stream);
            }

            // Buscar y eliminar el convenio
            var convenio = await _context.Convenios.FindAsync(id);
            if (convenio != null)
            {
                convenio.Eliminado = true; // Marca el convenio como eliminado
                _context.Update(convenio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Restore(int id)
        {
            var convenio = await _context.Convenios.FindAsync(id);
            if (convenio != null)
            {
                convenio.Eliminado = false;
                _context.Update(convenio);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Papelera));
        }

        // Método para verificar si un convenio existe
        private bool ConvenioExists(int id)
        {
            return _context.Convenios.Any(e => e.Id_Convenio == id);
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

        [HttpPost]
        public IActionResult ExportToExcel()
        {
            // Incluye las relaciones necesarias para obtener todos los datos
            var convenios = _context.Convenios
                .Include(c => c.Retribuciones)
                .Include(c => c.CentrosDeSalud)
                .Where(c => !c.Eliminado) // Puedes incluir los convenios eliminados si lo deseas
                .ToList();

            if (convenios == null || !convenios.Any())
            {
                // Si no hay datos, devuelve un mensaje al usuario
                TempData["ErrorMessage"] = "No hay convenios para exportar.";
                return RedirectToAction(nameof(Index));
            }

            // Configurar la licencia de EPPlus
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

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
                worksheet.Cells[1, 12].Value = "Adendum";
                worksheet.Cells[1, 13].Value = "Fecha Adendum";
                worksheet.Cells[1, 14].Value = "Observaciones Adendum";
                worksheet.Cells[1, 15].Value = "Tipo Retribución";
                worksheet.Cells[1, 16].Value = "Valor UF";
                worksheet.Cells[1, 17].Value = "Cantidad Pesos";
                worksheet.Cells[1, 18].Value = "Periodo";
                worksheet.Cells[1, 19].Value = "Tipo Práctica";
                worksheet.Cells[1, 20].Value = "Centro Salud - Nombre";
                worksheet.Cells[1, 21].Value = "Centro Salud - Dirección";
                worksheet.Cells[1, 22].Value = "Centro Salud - Contacto";
                worksheet.Cells[1, 23].Value = "Centro Salud - Correo";
                worksheet.Cells[1, 24].Value = "Centro Salud - Teléfono";

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
                    worksheet.Cells[row, 12].Value = convenio.Adendum ? "Sí" : "No";

                    // Agregar fecha de adendum y observaciones si Adendumn es verdadero
                    if (convenio.Adendum)
                    {
                        worksheet.Cells[row, 13].Value = convenio.Fecha_Termino?.ToShortDateString(); // Fecha de adendum
                        worksheet.Cells[row, 14].Value = convenio.Retribuciones?.FirstOrDefault()?.DetalleOtrosGastos ?? "N/A"; // Observaciones de adendum
                    }

                    // Agregar las retribuciones
                    if (convenio.Retribuciones != null && convenio.Retribuciones.Any())
                    {
                        foreach (var retribucion in convenio.Retribuciones)
                        {
                            worksheet.Cells[row, 15].Value = retribucion.Tipo_Retribucion;
                            worksheet.Cells[row, 16].Value = retribucion.Monto;
                            worksheet.Cells[row, 17].Value = retribucion.CantPesos;
                            worksheet.Cells[row, 18].Value = retribucion.Periodo;
                            worksheet.Cells[row, 19].Value = retribucion.Tipo_Practica;
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
                            worksheet.Cells[initialRow, 20].Value = centro.NombreCentro;
                            worksheet.Cells[initialRow, 21].Value = centro.Direccion;
                            worksheet.Cells[initialRow, 22].Value = centro.NombrecargocentroAso;
                            worksheet.Cells[initialRow, 23].Value = centro.CorreoPersonaCargo;
                            worksheet.Cells[initialRow, 24].Value = centro.Telefono_CentroAso;
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


    }
}
