using back.bbdd;
using back.dto;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;

namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly PeluqueriaDbContext _context;

        public AuthController(PeluqueriaDbContext context)
        {
            _context = context;
        }

        //POST: /api/usuario/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault( u=> u.Email == request.Email && u.Activo == true);
            
            if(usuario == null)
            {
                return Unauthorized("Usuario no encontrado");
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
            {
                return Unauthorized("La contraseña no es correcta");
            }

            return Ok(new
            {
                id = usuario.Id_Usuario,
                nombre= usuario.Nombre,
                email = usuario.Email,
                rol = usuario.Rol
            });
        }
    }
}