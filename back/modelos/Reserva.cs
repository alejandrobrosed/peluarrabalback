using back.modelos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.modelos
{
    public class Reserva {
        [Key]
        public int Id_Reserva { get; set; }
        [Required]
        public int Id_Cliente{ get; set; }
        [ForeignKey("Id_Cliente")]
        public Usuario Cliente { get; set; }
        [Required]
        public int Id_Servicio { get; set; }

        [ForeignKey("Id_Servicio")]
        public Servicio Servicio { get; set; }
        [Required]
        public int Id_Empleado { get; set; }

        [ForeignKey("Id_Empleado")]
        public Empleado Empleado { get; set; }
        [Required]
        public DateOnly Fecha { get; set; }
        [Required]
        public TimeOnly Hora_Inicio { get; set; }
        public String? Estado { get; set; }
        public String? Observaciones { get; set; }
    }
}