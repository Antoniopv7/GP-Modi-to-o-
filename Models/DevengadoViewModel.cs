using System;
using System.Collections.Generic;

namespace Gestion_Del_Presupuesto.Models
{
    public class DevengadoViewModel
    {
        // Propiedades adicionales para el filtrado en la vista
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime FechaFin { get; set; } = DateTime.Now;

        // Lista de Devengados que se mostrarán en la vista
        public IEnumerable<Devengado> Devengados { get; set; }
    }
}

