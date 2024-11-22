using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Gestion_Del_Presupuesto.Models
{
    public class Rol
    {
        [Key]
        public int Id_Rol { get; set; }

        public string? NombreRol { get; set; }

        public string Permisos { get; set; }

        // Relación con Usuario
        public virtual Usuario Usuarios { get; set; } // Relación uno a muchos con Usuario
    }

}
