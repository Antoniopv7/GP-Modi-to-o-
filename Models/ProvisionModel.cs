using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Gestion_Del_Presupuesto.Models
{
    public class ProvisionModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string NombreSede { get; set; } // "Santiago" o "Coquimbo"
        public string NombreCampoClinico { get; set; }
        public int Numero { get; set; }
        public string TipoGasto { get; set; } // "OC" o "GG"
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
