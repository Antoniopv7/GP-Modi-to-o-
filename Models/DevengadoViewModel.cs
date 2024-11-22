using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Gestion_Del_Presupuesto.Models
{
    public class DevengadoViewModel
    {
        // Propiedades adicionales para el filtrado en la vista
        [Key]
        public int Id_Devengadoview { get; set;}
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime FechaFin { get; set; } = DateTime.Now;

        // Lista de Devengados que se mostrarán en la vista
        public IEnumerable<Devengado> Devengados { get; set; }
    }
}

