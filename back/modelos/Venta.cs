using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace back.modelos

{
    public class Venta
    {
        [Key]
        public int Id_Venta { get; set; }
        [Required]
        public int Id_Cliente { get; set; }
        [ForeignKey("Id_Cliente")]
        public Usuario Cliente { get; set; }
        public DateTime? Fecha { get; set;}
        [Required]
        public decimal Total { get; set; }
    }   
}