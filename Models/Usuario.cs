using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gestion_Del_Presupuesto.Models
{
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }  
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public string Correo { get; set;}
        public string Rut { get; set; }
        public string Telefono { get; set; }
        public int Id_Rol { get; set; }
        public string ConfirmarPassword { get; set; }
        public virtual Rol Rol { get; set; }

        public ConvenioModel Convenios { get; set; }

        public virtual RegistroModel Registro { get; set; }
        public virtual ICollection<Historial_Actividad> Historial_Actividades { get; set; }  // Relación con Historial_Actividad

    }
}
