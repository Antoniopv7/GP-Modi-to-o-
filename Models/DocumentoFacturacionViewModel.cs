using System;

namespace Gestion_Del_Presupuesto.Models
{
    public class DocumentoFacturacionViewModel
    {
        public int Id_Factura { get; set; }
        public DateTime Fecha { get; set; }

        public DateTime FechaEmision { get; set; }
        public string Cliente { get; set; }
        public string Detalles { get; set; }
        public decimal Monto { get; set; }
        public decimal NumeroDocumento { get; set; }
        
        public decimal Institucion { get; set; }

        public string Descripcion { get; set; }

        public decimal TipoDocumento { get; set;}


    }
}
