using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class Devengado
    {
        [Key]
        public int Id_Devengado { get; set; }
        public string Carrera { get; set; }
        public string CentroCosto { get; set; }
        public string Itempresupuestario { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public decimal GastoTotalComprometidoMonto { get; set; }
        public int CantEstudiantes { get; set; }
        public decimal ValorUFDevengado { get; set; }

        public decimal CalcularDevengado()
        {
            return CantEstudiantes * ValorUFDevengado * (FechaFin - FechaInicio).Days;

        }
        public decimal SaldoPendiente { get; set; }
        public string Descripcion { get; set; }

        public decimal TotalGastoDevengadoGeneradoporEstudiantes { get; set; }

        // Relación con Convenio
        public int ConvenioId { get; set; }
        public virtual ConvenioModel Convenio { get; set; }

        // Relación con Planillas
        public virtual ICollection<PlanillasModel> Planillas { get; set; } = new List<PlanillasModel>();
    }
}