using back.bbdd;
using back.dto;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using back.servicios;

namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        //POST: /api/usuario/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _authService.GetUsuarioActivoPorEmail(request.Email);
            
            if(usuario == null)
            {
                return Unauthorized("Usuario no encontrado");
            }

            if(!_authService.VerificarPassword(request.Password, usuario.Password))
            {
                return Unauthorized("La contraseña no es correcta");
            }

            return Ok(new
            {
                id = usuario.Id_Usuario,
                nombre= usuario.Nombre,
                email = usuario.Email,
                avatarUrl = usuario.Avatar_Url,
                rol = usuario.Rol
            });
        }
    }
}