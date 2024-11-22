using Gestion_Del_Presupuesto.Data;
using Gestion_Del_Presupuesto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class FacturacionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FacturacionController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            // Obtener el usuario autenticado
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirigir al login si no está autenticado
            }

            // Obtener los datos del usuario que se mostraran en la vista
            ViewBag.NombreUsuario = user.UserName;
            ViewBag.EmailUsuario = user.Email;
            ViewBag.TelefonoUsuario = user.PhoneNumber;

            // Otros cálculos y lógica para facturaciones...
            var facturaciones = await _context.Facturacion.Include(f => f.Convenios).ToListAsync();

            // Calcula el Neto UF
            var netoUF = CalcularNetoUF(facturaciones);

            // Obtiene el valor de la UF actual (esperar la tarea)
            var valorUF = await ObtenerValorUFActual(DateTime.Now); // Aquí esperamos la tarea y pasamos la fecha deseada

            // Calcula el Total a Pagar
            var totalAPagar = CalcularTotalAPagar(netoUF, valorUF);

            // Pasa los valores calculados a la vista
            ViewBag.NetoUF = netoUF;
            ViewBag.TotalAPagar = totalAPagar;

            return View(facturaciones);
        }


        // GET: FacturacionController/Details/
        public ActionResult Details(int id)
        {
            var facturacion = _context.Facturacion.Include(f => f.Convenios).FirstOrDefault(f => f.Id_Facturacion == id);
            if (facturacion == null)
            {
                return NotFound();
            }
            return View(facturacion);
        }

        // GET: FacturacionController/Create
        public ActionResult Create()
        {
            ViewData["ConvenioId"] = new SelectList(_context.Convenios, "Id", "Nombre");
            return View();
        }

        // POST: FacturacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FacturacionModel facturacion)
        {

            _context.Facturacion.Add(facturacion);
            _context.SaveChanges();

            ViewData["ConvenioId"] = new SelectList(_context.Convenios, "Id", "Nombre", facturacion.ConvenioId);
            return View(facturacion);
        }

        // GET: FacturacionController/Edit/
        public ActionResult Edit(int id)
        {
            var facturacion = _context.Facturacion.Find(id);
            if (facturacion == null)
            {
                return NotFound();
            }
            ViewData["ConvenioId"] = new SelectList(_context.Convenios, "Id", "Nombre", facturacion.ConvenioId);
            return View(facturacion);
        }

        // POST: FacturacionController/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FacturacionModel facturacion)
        {
            if (id != facturacion.Id_Facturacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facturacion);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturacionExists(facturacion.Id_Facturacion))
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
            ViewData["ConvenioId"] = new SelectList(_context.Convenios, "Id", "Nombre", facturacion.ConvenioId);
            return View(facturacion);
        }

        // GET: FacturacionController/Delete/5
        public ActionResult Delete(int id)
        {
            var facturacion = _context.Facturacion.Include(f => f.Convenios).FirstOrDefault(f => f.Id_Facturacion == id);
            if (facturacion == null)
            {
                return NotFound();
            }
            return View(facturacion);
        }

        // POST: FacturacionController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var facturacion = _context.Facturacion.Find(id);
            _context.Facturacion.Remove(facturacion);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturacionExists(int id)
        {
            return _context.Facturacion.Any(e => e.Id_Facturacion == id);
        }

        public decimal CalcularNetoUF(List<FacturacionModel> facturaciones)
        {
            decimal netoUF = 0;

            foreach (var facturacion in facturaciones)
            {
                // Cálculo del subtotal por cada fila
                var subtotal = facturacion.NumeroTiempo * facturacion.NumeroAlumnos * facturacion.ValorUFMesPractica;
                facturacion.Subtotal = subtotal; // Actualiza el valor del subtotal en el modelo
                netoUF += subtotal;
            }

            return netoUF;
        }

        public decimal CalcularTotalAPagar(decimal netoUF, decimal valorUF)
        {
            return netoUF * valorUF;
        }
        public async Task<JsonResult> BuscarUF(DateTime SelectedDate)
        {
            string url = $"https://si3.bcentral.cl/SieteRestWS/SieteRestWS.ashx?user=Ruben1Ulloa@gmail.com&pass=Benchi12&function=GetSeries&timeseries=F073.UFF.PRE.Z.D&firstdate={SelectedDate:yyyy-MM-dd}&lastdate={SelectedDate:yyyy-MM-dd}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        IndicadorEconomico indicador = JsonConvert.DeserializeObject<IndicadorEconomico>(jsonResponse);

                        // Verificar que Series y Obs no sean null antes de acceder a ellas
                        if (indicador?.Series?.Obs != null && indicador.Series.Obs.Any())
                        {
                            var valorUF = indicador.Series.Obs.FirstOrDefault()?.Value;
                            var fechaUF = indicador.Series.Obs.FirstOrDefault()?.IndexDateString;
                            return Json(new { valorUF = valorUF, fechaUF = fechaUF });
                        }
                        else
                        {
                            return Json(new { valorUF = 0, error = "No se encontraron datos de UF para la fecha seleccionada." });
                        }
                    }
                    else
                    {
                        return Json(new { valorUF = 0, error = "Error al obtener el valor de la UF: " + response.StatusCode });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { valorUF = 0, error = "Error de conexión: " + ex.Message });
                }
            }
        }
        public async Task<decimal> ObtenerValorUFPromedio(DateTime fechaInicio, DateTime fechaFin)
        {
            string url = $"https://si3.bcentral.cl/SieteRestWS/SieteRestWS.ashx?user=Ruben1Ulloa@gmail.com&pass=Benchi12&function=GetSeries&timeseries=F073.UFF.PRE.Z.D&firstdate={fechaInicio:yyyy-MM-dd}&lastdate={fechaFin:yyyy-MM-dd}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        IndicadorEconomico indicador = JsonConvert.DeserializeObject<IndicadorEconomico>(jsonResponse);

                        if (indicador?.Series?.Obs != null && indicador.Series.Obs.Any())
                        {
                            var valoresUF = indicador.Series.Obs.Select(obs => Convert.ToDecimal(obs.Value)).ToList(); // Convierte a decimal
                            if (valoresUF.Any())
                            {
                                return valoresUF.Average();  // Calcula el promedio de los valores de UF
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error de conexión: " + ex.Message);
                }
            }

            return 0; // Valor por defecto si no se puede obtener la UF
        }
        public async Task<decimal> ObtenerValorUFActual(DateTime selectedDate)
        {
            string url = $"https://si3.bcentral.cl/SieteRestWS/SieteRestWS.ashx?user=Ruben1Ulloa@gmail.com&pass=Benchi12&function=GetSeries&timeseries=F073.UFF.PRE.Z.D&firstdate={selectedDate:yyyy-MM-dd}&lastdate={selectedDate:yyyy-MM-dd}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        IndicadorEconomico indicador = JsonConvert.DeserializeObject<IndicadorEconomico>(jsonResponse);

                        if (indicador?.Series?.Obs != null && indicador.Series.Obs.Any())
                        {
                            var valorUFString = indicador.Series.Obs.FirstOrDefault()?.Value;
                            if (decimal.TryParse(valorUFString, out decimal valorUF))
                            {
                                return valorUF;  // Devuelve el valor de la UF convertido correctamente
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error de conexión: " + ex.Message);
                }
            }

            return 0; //  si no se puede obtener la UF
        }

    }
}