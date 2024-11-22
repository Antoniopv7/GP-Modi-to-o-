using Gestion_Del_Presupuesto.Models;

namespace Gestion_Del_Presupuesto.Services.Interfaces
{
    public interface IFacturacionService
    {
        decimal CalcularSubtotal(int numAlumnos, int numTiempo, decimal valorUFMesPractica);
        decimal CalcularNetoUF(List<FacturacionModel> facturacionesSeleccionadas);
        decimal CalcularTotalAPagar(decimal netoUF, decimal valorUF);
    }
}
