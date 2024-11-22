
using Gestion_Del_Presupuesto.Models;
using Gestion_Del_Presupuesto.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Gestion_Del_Presupuesto.Services
{
    public class FacturacionService : IFacturacionService
    {
        public decimal CalcularSubtotal(int numAlumnos, int numTiempo, decimal valorUFMesPractica)
        {
            return numAlumnos * numTiempo * valorUFMesPractica;
        }

        public decimal CalcularNetoUF(List<FacturacionModel> facturacionesSeleccionadas)
        {
            return facturacionesSeleccionadas.Sum(f => f.Subtotal);
        }

        public decimal CalcularTotalAPagar(decimal netoUF, decimal valorUF)
        {
            return netoUF * valorUF;
        }
    }
}
