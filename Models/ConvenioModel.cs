using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Del_Presupuesto.Models
{
    public class ConvenioModel
    {
        [Key]
        public int Id_Convenio { get; set; }
        [Display(Name = "Nombre Institución")]
        public string Nombre { get; set; }

        [Display(Name = "Tipo Centro")]

        public string? Tipo_Convenio { get; set; }

        public string Sede { get; set; }
        [Display(Name = "Fecha Inicio")]

        public DateTime Fecha_Inicio { get; set; } = DateTime.Now;

        [Display(Name = "Fecha Termino")]
        public DateTime? Fecha_Termino { get; set; } = DateTime.Now;

        [Display(Name = "Contacto Principal")]
        public string ContactoPrincipal { get; set; }
        public string Telefono { get; set; }
        public string Rut { get; set; }
        public string Direccion { get; set; }
        public bool RenovacionAutomatica { get; set; }
        public bool Adendum { get; set; }
        public DateTime? FechaAdendum { get; set; } = DateTime.Now;
        public string? ObservacionAdendum { get; set; }
        public int Version { get; set; } = 1;

        public decimal ValorUF { get; set; }
        public bool Eliminado { get; set; }
        public int Id_Retribucion { get; set; }
        public int CentrosDeSaludId { get; set; }

        public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

        public List<RetribucionModel> Retribuciones { get; set; } = new List<RetribucionModel>();
        public List<CentroSaludModel> CentrosDeSalud { get; set; } = new List<CentroSaludModel>();
        public virtual ICollection<Devengado> Devengados { get; set; } = new List<Devengado>();

        // Relación con Facturación
        public virtual ICollection<FacturacionModel> Facturacion { get; set; } = new List<FacturacionModel>();
    }

}
