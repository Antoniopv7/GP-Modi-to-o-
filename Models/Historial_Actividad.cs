using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Del_Presupuesto.Models
{
    public class Historial_Actividad
    {
        [Key]
        public int Id_Historial_Actividad { get; set; }  // Clave primaria de Historial_Actividad

        public DateTime Fecha { get; set; }
        public string Accion { get; set; }

        // Clave foránea hacia Usuario
        public int Id_Usuario { get; set; }

        public virtual Rol Roles { get; set; }

        // Propiedad de navegación hacia Usuario
        [ForeignKey("Id_Usuario")]
        public virtual Usuario Usuarios { get; set; }
    }
}
