using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class LoginViewModel
    {
        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        public bool Recordarme { get; set; }
    }
}
