using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using back.modelos;

namespace back.modelos

{
    public class Empleado
    {
        [Key]
        public int Id_Empleado { get; set; }
        [Required]
        public int Id_Usuario { get; set; }
        [ForeignKey("Id_Usuario")]
        public Usuario Usuario { get; set; }
        public String? Especialidad { get; set; }

        public ICollection<Reserva>? Reservas {get; set;}
    }    
}