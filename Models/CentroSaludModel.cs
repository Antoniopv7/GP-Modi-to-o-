using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Del_Presupuesto.Models
{
    public class CentroSaludModel
    {
        [Key]
        public int Id_CentroSalud { get; set; }
        public int Rut_CentrodeSalud { get; set; }

        [Required]
        [Display(Name = "Nombre Centro de Salud Asociado")]
        public string NombreCentro { get; set; }
        public string Direccion { get; set; }

        [Display(Name = "Telefono Centro de Salud Asociado")]
        public string Telefono_CentroAso { get; set; }

        public string NombrecargocentroAso { get; set; }

        public string CorreoPersonaCargo { get; set; }

        [ForeignKey("ConvenioModel")]
        public int ConvenioId { get; set; }
        public ConvenioModel? Convenios { get; set; }
        public RetribucionModel? Retribuciones { get; set; }
    }
}
