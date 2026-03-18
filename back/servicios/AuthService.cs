using back.bbdd;
using back.modelos;

namespace back.servicios
{
    public class AuthService
    {
        private readonly PeluqueriaDbContext _context;

        public AuthService(PeluqueriaDbContext context)
        {
            _context = context;
        }

        public Usuario? GetUsuarioActivoPorEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Activo == true);
        }

        public bool VerificarPassword(string passwordPlano, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(passwordPlano, passwordHash);
        }
     }
}
