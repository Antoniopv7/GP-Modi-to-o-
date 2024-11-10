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

        public DateTime FechaInicio { get; set; } = DateTime.Now;

        public DateTime FechaFin { get; set; } = DateTime.Now;

        public DateTime CantidadTiempo { get; set; }


        public decimal GastoTotalComprometidoMonto { get; set; }

        public int CantEstudiantes { get; set; }

        public int ValorUFDevengado { get; set; }

        public int CostoUF { get; set; }
        

        public decimal PagosRealizados { get; set; }
        public decimal SaldoPendiente { get; set; }

        public string Descripcion { get; set; }

        public decimal TotalGastoDevengadoGeneradoporEstudiantes { get; set; }

        public int ConvenioId { get; set; }
        public virtual ConvenioModel Convenio { get; set; }
        public IEnumerable<Devengado> Devengados { get; set; }
        public virtual ObsData ObsvalorUF { get; set; }

    }
}
