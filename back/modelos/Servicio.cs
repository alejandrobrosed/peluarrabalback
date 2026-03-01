using System.ComponentModel.DataAnnotations;
namespace back.modelos

{
    public class Servicio {

        [Key]
        public int Id_Servicio { get; set; }
        [Required]
        public String Nombre { get; set; }
        public String? Descripcion { get; set; }
        [Required]
        public int Duracion_Minutos { get; set; }
        [Required]
        public decimal Precio { get; set;}
        public bool Activo { get; set; }
    }    
}