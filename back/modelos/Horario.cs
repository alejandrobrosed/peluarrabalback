using back.modelos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.modelos

{
    public class Horario {
        [Key]
        public int Id_Horario { get; set; }
        [Required]
        public int Id_Empleado { get; set; }
        [ForeignKey("Id_Empleado")]
        public Empleado Empleado { get; set; }
        [Required]
        public string Dia_Semana { get; set; }
        [Required]
        public TimeOnly Hora_Inicio { get; set; }
        [Required]
        public TimeOnly Hora_Fin { get; set; }
    }
}