using System.ComponentModel.DataAnnotations;
namespace back.modelos
{
    public class Producto {
        [Key]
        public int Id_Producto { get; set; }
        [Required]
        public String Nombre { get; set; }
        public String? Marca { get; set; }
        public String? Descripcion { get; set;}
        [Required]
        public decimal Precio_Venta { get; set;}
        public int? Stock { get; set; }
        public bool? Activo { get; set; }
    }
}