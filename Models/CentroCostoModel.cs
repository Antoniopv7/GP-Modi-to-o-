using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class CentroCostoModel
    {
        [Key]
        public int Id_CentroCosto { get; set; }
        public string NombreCentro { get; set; }
        public string CodigoCentro { get; set; } // Ejemplo: "ACD209171"

        // Relación con carreras/sedes
        public string Carrera { get; set; }
        public string Sede { get; set; }

        // Campos adicionales
        public double CostoMM { get; set; }
        public double RRHHxRetribucion { get; set; }
        public double Capacitacion { get; set; }
        public double PagoApoyoDocencia { get; set; }
        public double OtrosGastosRetribucion { get; set; }

        // Campos financieros
        public double TotalDeuda { get; set; }
        public double TotalFacturado { get; set; }
        public double SaldoEstimado { get; set; }
    }
}
