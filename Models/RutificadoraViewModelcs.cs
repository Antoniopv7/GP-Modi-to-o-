using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class RutificadoraModel
    {
        [Key]
        public int Id_Rutificadora { get; set; }  // Identificador único para la tabla

        [Required]
        [Display(Name = "Asignatura")]
        public string Asignatura { get; set; }

        [Required]
        [Display(Name = "Institución")]
        public string Institucion { get; set; }

        [Required]
        [Display(Name = "Nombre Completo (Apellidos, Nombres)")]
        public string NombreCompleto { get; set; }

        [Required]
        [Display(Name = "RUT")]
        public string RUT { get; set; }

        [Required]
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Display(Name = "Fecha de Término")]
        [DataType(DataType.Date)]
        public DateTime FechaTermino { get; set; }

        [Required]
        [Display(Name = "Número de Días")]
        public int NumeroDias { get; set; }

        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }
    }
}

