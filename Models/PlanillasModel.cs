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
        public string Institución { get; set; }
        public string Nombre { get; set; }
        public string Rut { get; set; }

        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Termino { get; set; }
        public double CantidadHoras { get; set; }
        public int CantDias { get; set; }
        public string Observaciones { get; set; }

        // Relación con Estudiante
        public int Id_Estudiante { get; set; }
        public virtual Estudiante Estudiante { get; set; }

        // Relación con Carrera
        public int CarreraId { get; set; }
        public virtual CarreraModel Carrera { get; set; }

        // Relación con Facturación
        public int FacturacionId { get; set; }
        public virtual FacturacionModel Facturacion { get; set; }

        // Relación con Devengado
        public virtual Devengado Devengado { get; set; }

        // Relación con Indicador Económico (ejemplo: valor UF)
        public int Id_IndicadorEco { get; set; }
        public virtual IndicadorEconomico Indicador { get; set; }
    }

}
