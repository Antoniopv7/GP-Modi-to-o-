using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class Estudiante
    {
        [Key]
        public int Id_Estudiante { get; set; }
        public string Nombre { get; set; }
        public string Carrera { get; set; }

        public int Id_Convenio { get; set; }

        public virtual ICollection <ConvenioModel> Convenio { get; set; }

        public virtual PlanillasModel Planilla { get; set; }
    }
}
