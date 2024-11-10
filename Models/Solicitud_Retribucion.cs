using System;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class Solicitud_Retribucion
    {
        [Key]
        public int Id_Solicitud_Retribucion { get; set; }
        public DateTime Fecha { get; set; }
        public string Solicitante { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }

        // Foreign Key a Convenio
        public int Id_Convenio { get; set; }
        public virtual ConvenioModel Convenio { get; set; }
        public virtual RetribucionModel Retribucion { get; set; }
    }
}

