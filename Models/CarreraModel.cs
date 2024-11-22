using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Del_Presupuesto.Models
{
    public class CarreraModel
    {
        [Key]
        public int Id_Carrera { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Facultad { get; set; }

        // Relación con estudiantes
        public ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

        // Relación con planillas
        public ICollection<PlanillasModel> Planillas { get; set; } = new List<PlanillasModel>();
    }
}
