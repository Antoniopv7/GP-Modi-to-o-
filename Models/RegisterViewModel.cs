using System.ComponentModel.DataAnnotations;
using Gestion_Del_Presupuesto.Models;

namespace Gestion_Del_Presupuesto.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }

        [Required]
        [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmarContraseña { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public int Id_Rol { get; set; }

        // Propiedades adicionales para Rut y Teléfono
        [Required]
        [Display(Name = "RUT")]
        public string Rut { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Número de Teléfono")]
        public string Telefono { get; set; }

        // Lista de roles para mostrar en el formulario
        public List<Rol> Roles { get; set; }
    }
}
