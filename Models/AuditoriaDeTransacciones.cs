using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class AuditoriaDeTransacciones
    {
        
        [Key]
        public int Id_AuditoriaDeTransacciones { get; set; }


        public string Sede { get; set; }
        public string Tipo_Costo { get; set; }
        public int Monto { get; set; }

        public string Descripcion { get; set; }

        public string Fecha { get; set; }

        // Relación muchos a muchos con Estudiante
        public virtual ICollection<Estudiante> Estudiantes { get; set; }

        // Relación con Convenio
        public virtual ICollection<ConvenioModel> Convenios { get; set; }
    }
}

