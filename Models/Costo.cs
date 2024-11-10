using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Del_Presupuesto.Models
{
    public class Costo
    {
        [Key]
        public int Id_Costo { get; set; }

        [Required]
        public float Monto { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public string Tipo { get; set; }

        // Relación con Convenio
        public int Id_Convenio { get; set; }

        [ForeignKey("Id_Convenio")]
        public virtual ConvenioModel Convenio { get; set; }

        // Descripción adicional del costo
        public string Descripcion { get; set; }
    }
}

