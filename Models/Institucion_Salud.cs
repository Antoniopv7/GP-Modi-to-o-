using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class Institucion_Salud
    {
        [Key]
        public int Id_Institucion_Salud { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        // Relación con Convenio
        public virtual ICollection<ConvenioModel> Convenios { get; set; }
    }
}

