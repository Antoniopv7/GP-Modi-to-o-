using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class Estudiante
    {
        [Key]
        public int Id_Estudiante { get; set; }
        public string Nombre { get; set; }
        public string Rut_Estudiante { get; set; }

        public int Nivel_Formacion { get; set; }

        // Relación con Carrera
        public int CarreraId { get; set; }
        public virtual CarreraModel Carrera { get; set; }

        // Relación con Convenio
        public int Id_Convenio { get; set; }
        public virtual ConvenioModel Convenio { get; set; }

        // Relación con Planilla
        public virtual PlanillasModel Planilla { get; set; }
    }
}
