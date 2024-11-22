using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Del_Presupuesto.Models
{
    public class RegistroModel
    {
        [Key]
        public int Id_Registro { get; set; }
        public int Id_Usuarios { get; set; }
        public virtual Usuario Usuarios { get; set; } 

    }
}
