using System.ComponentModel.DataAnnotations;
namespace back.modelos

{
    public class Venta
    {
        [Key]
        public int Id_Venta { get; set; }
        [Required]
        public int Id_Cliente { get; set; }
        [Required]
        public Usuario Cliente { get; set; }
        public DateTime? Fecha { get; set;}
        [Required]
        public decimal Total { get; set; }
    }   
}