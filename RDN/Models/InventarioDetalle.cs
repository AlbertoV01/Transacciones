//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RDN.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InventarioDetalle
    {
        public int Folio { get; set; }
        public string Sucursal { get; set; }
        public int Renglon { get; set; }
        public string ProductoID { get; set; }
        public decimal Precio { get; set; }
        public double Cantidad { get; set; }
    
        public virtual Inventarios Inventarios { get; set; }
        public virtual Productos Productos { get; set; }
        public virtual Saldos Saldos { get; set; }
    }
}
