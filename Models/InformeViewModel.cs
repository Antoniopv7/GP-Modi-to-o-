using System;
using System.Collections.Generic;

namespace Gestion_Del_Presupuesto.Models
{
    public class InformeViewModel
    {
        public string Titulo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<string> Detalles { get; set; }
        public string TipoInforme { get; set; }
        public string Carrera { get; set; }
        public string TipoReporte { get; set;}
    }

}
