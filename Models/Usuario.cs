using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Gestion_Del_Presupuesto.Models
{
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }

        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Contraseña { get; set; }

        [Required]
        public int Id_Rol { get; set; }

        // Nuevas propiedades
        [Required]
        [Display(Name = "RUT")]
        public string Rut { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Número de Teléfono")]
        public string Telefono { get; set; }

        // Relación con el rol
        public Rol Rol { get; set; }

        public virtual ICollection<Historial_Actividad> Historial_Actividades { get; set; }  // Relación con Historial_Actividad
    }
}
