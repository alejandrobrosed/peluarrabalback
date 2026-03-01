using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace back.modelos
{
    public class VentaDetalle
    {
        [Key]
        public int Id_Detalle { get; set; }
        [Required]
        public int Id_Venta { get; set; }
        [ForeignKey("Id_Venta")]
        public Venta Venta { get; set; }
        [Required]
        public int Id_Producto { get; set; }
        [ForeignKey("Id_Producto")]
        public Producto Producto { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal PrecioUnitario { get; set;}
    }
}