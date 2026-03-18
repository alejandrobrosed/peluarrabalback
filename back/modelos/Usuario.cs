using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace back.modelos

{
    public class Usuario {
        [Key]
        public int Id_Usuario { get; set; }
        [Required]
        public String Nombre { get; set; }
        [Required]
        public String Apellidos { get; set; }
        [Required]
        public String Email { get; set; }
        public String? Telefono { get; set; }
        [Required]
        public String Password { get; set;}
        [Column("avatar_url")]
        public String? Avatar_Url { get; set; }
        public String? Rol { get; set; }
        public bool? Activo { get; set; }
    }    
}