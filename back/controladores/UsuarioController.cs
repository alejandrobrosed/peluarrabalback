using back.modelos;
using back.bbdd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using back.dto;
using back.servicios;
using Microsoft.Extensions.Logging;

namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController: ControllerBase
    {
        private readonly PeluqueriaDbContext _context;
        private readonly UsuariosService _usuariosService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(PeluqueriaDbContext context, UsuariosService usuariosService, ILogger<UsuarioController> logger)
        {
            _context = context;
            _usuariosService = usuariosService;
            _logger = logger;
        }

        // GET: /api/usuarios
        [HttpGet]
        public IActionResult GetUsuarios([FromQuery] string? rol)
        {

            var query = _context.Usuarios.AsQueryable();
            if (!string.IsNullOrEmpty(rol))
            {
                query = query.Where(u => u.Rol == rol);
            }

            var usuarios = query
                .Select(u => new
                {
                    u.Id_Usuario,
                    u.Nombre,
                    u.Apellidos,
                    u.Email,
                    u.Telefono,
                    u.Avatar_Url,
                    u.Rol,
                    u.Activo
                })
                .ToList();
                return Ok(usuarios);
        }

        //GET: /api/usuarios/clientes
        [HttpGet("clientes")]
        public IActionResult GetClientes()
        {
            var clientes = _context.Usuarios
                .Where(u => u.Rol == "cliente" && u.Activo == true)
                .ToList();
           
            return Ok(clientes);
        }

        // GET: /api/usuarios/3
        [HttpGet("{id}")]
        public IActionResult GetUsuarios(int id)
        {
            var usuarios = _context.Usuarios
                .Where(u => u.Id_Usuario == id)
                .Select(u => new
                {
                    u.Id_Usuario,
                    u.Nombre,
                    u.Apellidos,
                    u.Email,
                    u.Telefono,
                    u.Avatar_Url,
                    u.Rol,
                    u.Activo
                })
                .FirstOrDefault();

            if(usuarios == null)
            {
                return NotFound();
            }
            return Ok(usuarios);
        }

        //POST: /api/usuarios
        [HttpPost]
        public IActionResult CrearUsuario([FromBody] Usuario usuario)
        {
            usuario.Rol = "cliente";
            usuario.Activo = true;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUsuarios), new {id = usuario.Id_Usuario}, usuario);
        }

        //PUT: /api/usuario/5
        [HttpPut("{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] Usuario usuarioActualizado)
        {
            var usuario = _context.Usuarios.Find(id);
            if(usuario == null)
            {
                return NotFound();
            }
            usuario.Nombre = usuarioActualizado.Nombre;
            usuario.Apellidos = usuarioActualizado.Apellidos;
            usuario.Email = usuarioActualizado.Email;
            usuario.Telefono = usuarioActualizado.Telefono;

            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: /api/usuario/5
        [HttpDelete("{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if(usuario == null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(usuario);

            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: /api/usuario/5
        [HttpDelete("{id}/desactivar")]
        public IActionResult DesactivarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if(usuario == null)
            {
                return NotFound();
            }
            usuario.Activo = false;

            _context.SaveChanges();
            return NoContent();
        }

        //POST: /api/usuario/5/avatar
        [HttpPost("{id}/avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SubirAvatar(int id, IFormFile file)
        {
            try
            {
                var url = await _usuariosService.SubirAvatarAsync(id, file);
                if (url == null)
                {
                    return NotFound();
                }

                return Ok(new { avatarUrl = url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subiendo avatar a Cloudinary para el usuario {UserId}", id);
                return BadRequest(ex.Message);
            }
        }

    }
}