using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class Pago
    {
        [Key]
        public int Id_Pago { get; set; }
        public DateTime Fecha { get; set; }

        public DateTime FechaVencimiento { get; set; }
        public float Monto { get; set; }

        public string Nombre { get; set; }
        public string Estado { get; set; }

        // Foreign Key
        public int Id_Retribucion { get; set; }
        public int Id_Convenio { get; set; }

        // Propiedad de navegación
        public virtual RetribucionModel Retribuciones { get; set; }
        public virtual ConvenioModel Convenios { get; set; }
    }
}
