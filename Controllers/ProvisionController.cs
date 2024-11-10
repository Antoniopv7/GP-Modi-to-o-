using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Gestion_Del_Presupuesto.Models;

public class ProvisionController : Controller
{
    private static decimal PresupuestoTotalInicial = 500000; // Ejemplo de presupuesto inicial
    private static List<ProvisionModel> Provisiones = new List<ProvisionModel>(); // Lista de provisiones en memoria

    // Acción para ver el listado de provisiones
    public IActionResult Index()
    {
        ViewBag.PresupuestoTotalInicial = PresupuestoTotalInicial;
        ViewBag.TotalGastado = Provisiones.Sum(p => p.Monto);
        ViewBag.PresupuestoRestante = PresupuestoTotalInicial - ViewBag.TotalGastado;

        return View(Provisiones);
    }

    // Acción para mostrar la vista de creación
    public IActionResult Create()
    {
        return View();
    }

    // Acción POST para agregar una nueva provisión
    [HttpPost]
    public IActionResult Create(ProvisionModel provision)
    {
        if (ModelState.IsValid)
        {
            provision.Id = Provisiones.Count > 0 ? Provisiones.Max(p => p.Id) + 1 : 1;
            Provisiones.Add(provision);
            return RedirectToAction("Index");
        }

        return View(provision);
    }

    // Acción para editar una provisión
    public IActionResult Edit(int id)
    {
        var provision = Provisiones.FirstOrDefault(p => p.Id == id);
        if (provision == null)
            return NotFound();

        return View(provision);
    }

    // Acción POST para actualizar la provisión
    [HttpPost]
    public IActionResult Edit(ProvisionModel provision)
    {
        var existingProvision = Provisiones.FirstOrDefault(p => p.Id == provision.Id);
        if (existingProvision == null)
            return NotFound();

        existingProvision.Fecha = provision.Fecha;
        existingProvision.NombreSede = provision.NombreSede;
        existingProvision.NombreCampoClinico = provision.NombreCampoClinico;
        existingProvision.Numero = provision.Numero;
        existingProvision.TipoGasto = provision.TipoGasto;
        existingProvision.Descripcion = provision.Descripcion;
        existingProvision.Monto = provision.Monto;

        return RedirectToAction("Index");
    }

    // Acción para eliminar una provisión
    public IActionResult Delete(int id)
    {
        var provision = Provisiones.FirstOrDefault(p => p.Id == id);
        if (provision == null)
            return NotFound();

        Provisiones.Remove(provision);
        return RedirectToAction("Index");
    }
}