using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Del_Presupuesto.Models
{
    public class FacturacionModel
    {
        [Key]
        public int Id_Facturacion { get; set; }

        // Nuevos campos agregados
        [Required]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        [Required]
        [Display(Name = "RUT")]
        public string RUT { get; set; }

        [Required]
        [Display(Name = "Giro")]
        public string Giro { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required]
        [Display(Name = "Receptor de Documento")]
        public string ReceptorDocumento { get; set; }

        [Required]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; }

        [Required]
        [Display(Name = "Teléfono de Receptor")]
        public string TelefonoReceptor { get; set; }

        [Required]
        [Display(Name = "Correo Electrónico")]
        public string CorreoElectronico { get; set; }

        [Required]
        [Display(Name = "Sede")]
        public string Sede { get; set; }

        // Campos anteriores
        [Required]
        [Display(Name = "Carrera")]
        public string Carrera { get; set; }

        [Required]
        [Display(Name = "Nivel Formación")]
        public string NivelFormacion { get; set; }

        [Required]
        [Display(Name = "Institución")]
        public string Institucion { get; set; }

        [Required]
        [Display(Name = "Tiempo de práctica")]
        public string TiempoPractica { get; set; }

        [Required]
        [Display(Name = "N° mes/día/semana/hora")]
        public int NumeroTiempo { get; set; }

        [Required]
        [Display(Name = "N° Alumnos")]
        public int NumeroAlumnos { get; set; }

        [Required]
        [Display(Name = "Valor 3 UF mes/práctica profesional")]
        public decimal ValorUFMesPractica { get; set; } 
        public DateTime FechaUFDia { get; set; } = DateTime.Now;

        [Required]
        public decimal ValorUF { get; set; }

        [Required]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        public int TotalEstudiantes { get; set; }

        public decimal NetoUF { get; set; }

        public decimal TotalaPagar { get; set; }


        [ForeignKey("ConvenioModel")]
        [Required]
        public int ConvenioId { get; set; }

        public int Id_IndicadorEco { get; set; }
        public ConvenioModel? Convenios { get; set; }
        public PlanillasModel? Planillas { get; set; }
        public ObsData? ObsvalorUF { get; set; }
        public IndicadorEconomico? Indicador { get; set; }


    }
}
