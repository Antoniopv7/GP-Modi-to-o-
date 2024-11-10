using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Del_Presupuesto.Models
{
    public class PresupuestoModel
    {
        [Key]
        public int Id_PresupuestoXCentroCosto { get; set; }

        [Required]
        [Display(Name = "Carrera")]
        public string Carrera { get; set; }

        [Required]
        [Display(Name = "Sede")]
        public string Sede { get; set; }

        [Required]
        [Display(Name = "Convenio")]
        public string Convenio { get; set; }

        [Required]
        [Display(Name = "Centro de Costo")]
        public string CentroCosto { get; set; }

        [Required]
        [Display(Name = "Costo $ / MM")]
        public decimal CostoMM { get; set; }

        [Required]
        [Display(Name = "RRHH x retribución")]
        public decimal RRHHRetribucion { get; set; }

        [Required]
        [Display(Name = "Capacitación x retribución")]
        public decimal CapacitacionRetribucion { get; set; }

        [Required]
        [Display(Name = "Pago Apoyo a la Docencia")]
        public decimal PagoApoyoDocencia { get; set; }

        [Required]
        [Display(Name = "Otros Gastos x retribución")]
        public decimal OtrosGastosRetribucion { get; set; }

        [Required]
        [Display(Name = "Total Gasto estimado")]
        public decimal TotalGastoEstimado { get; set; }

        [Required]
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Display(Name = "Deuda Morosa")]
        public decimal? DeudaMorosa { get; set; }

        [Display(Name = "Deuda Año Actual")]
        public decimal? DeudaAnioActual { get; set; }

        [Display(Name = "Total Deuda")]
        public decimal? TotalDeuda { get; set; }

        [Display(Name = "Facturado 2024 corresponsal a 2020, 2021, 2022")]
        public decimal? FacturadoAniosAnteriores { get; set; }

        [Display(Name = "Facturado 2024 corresponsal a 2023")]
        public decimal? FacturadoAnioActual { get; set; }

        [Display(Name = "Facturado 2024 corresponsal a 2024")]
        public decimal? FacturadoCorresponde2024 { get; set; }

        [Display(Name = "TOTAL FACTURADO Y PAGADO")]
        public decimal? TotalFacturadoPagado { get; set; }

        [Display(Name = "Saldo Estimado x pagar Año Actual")]
        public decimal? SaldoEstimadoPagarAnioActual { get; set; }

        [ForeignKey("ConvenioModel")]
        [Required]
        public int ConvenioId { get; set; }
        public ConvenioModel? Convenios { get; set; }
        public List<string> FormasRetribucionSeleccionadas { get; set; } = new List<string>();  // Almacena las retribuciones seleccionadas
    }
}
