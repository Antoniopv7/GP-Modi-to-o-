using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class ConvenioModel
    {
        [Key]
        public int Id_Convenio { get; set; }

        [Display(Name = "Nombre Institución")]
        [Required(ErrorMessage = "El Nombre de la Institución es obligatorio.")]
        public string Nombre { get; set; }

        [Display(Name = "Tipo Centro")]
        [Required(ErrorMessage = "El Tipo de Convenio es obligatorio.")]
        public string Tipo_Convenio { get; set; }

        [Required(ErrorMessage = "La Sede es obligatoria.")]
        public string Sede { get; set; }

        [Display(Name = "Fecha Inicio")]
        [Required(ErrorMessage = "La Fecha de Inicio es obligatoria.")]
        public DateTime Fecha_Inicio { get; set; } = DateTime.Now;

        [Display(Name = "Fecha Termino")]
        public DateTime? Fecha_Termino { get; set; }

        [Display(Name = "Contacto Principal")]
        [Required(ErrorMessage = "El Contacto Principal es obligatorio.")]
        public string ContactoPrincipal { get; set; }

        [Required(ErrorMessage = "El Teléfono es obligatorio.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El RUT es obligatorio.")]
        public string Rut { get; set; }

        [Required(ErrorMessage = "La Dirección es obligatoria.")]
        public string Direccion { get; set; }

        [Display(Name = "Renovación Automática")]
        public bool RenovacionAutomatica { get; set; }

        [Display(Name = "Valor UF")]
        [Required(ErrorMessage = "El Valor UF es obligatorio.")]
        public decimal ValorUF { get; set; }

        public bool Eliminado { get; set; }

        // Relaciones
        public List<RetribucionModel> Retribuciones { get; set; } = new List<RetribucionModel>();
        public List<CentroSaludModel> CentrosDeSalud { get; set; } = new List<CentroSaludModel>();
    }
}
