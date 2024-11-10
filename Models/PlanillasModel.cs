using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System;

namespace Gestion_Del_Presupuesto.Models
{
    public class PlanillasModel
    {
        [Key]
        public int Id_Planillas { get; set; }

        public string Asignatura { get; set; }

        [Display(Name = "Nombre Institución")]
        public string Institución { get; set; }

        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; }

        [Display(Name = "Rut Estudiantes")]
        public int  Rut { get; set; }

        public DateTime Fecha_Inicio { get; set; } = DateTime.Now;

        public required DateTime Fecha_Termino { get; set; } = DateTime.Now;


        [Display(Name = "Cantidad de Horas")]
        public double CantidadHoras {  get; set; }

        public int CantDias { get; set; }

        public string Observaciones { get; set; }
        public int Id_Estudiante {  get; set; }

        public virtual Estudiante Estudiante { get; set; }


        
    }
}
