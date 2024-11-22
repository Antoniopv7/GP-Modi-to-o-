using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Del_Presupuesto.Models
{
    public class RetribucionModel
    {
        [Key]
        public int Id_Retribucion { get; set; }

        [Required]
        public string? Tipo_Retribucion { get; set; }
        public string? DetalleOtrosGastos { get; set; }
        public decimal Monto { get; set; }
        public decimal CantPesos { get; set; }
        public decimal UFTotal { get; set; }

        [ForeignKey("ConvenioModel")]
        [Required]
        public int ConvenioId { get; set; }

        [ForeignKey("CarreraModel")]
       
        public int CarreraId { get; set; }
        public string? Periodo { get; set; }
        public string? Tipo_Practica { get; set; }

        public DateTime FechaRetribucion { get; set; } = DateTime.Now;
        public ConvenioModel? Convenios { get; set; }
        
    }
}
