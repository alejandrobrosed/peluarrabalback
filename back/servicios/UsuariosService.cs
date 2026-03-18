using back.bbdd;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace back.servicios
{
    public class UsuariosService
    {
        private readonly PeluqueriaDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public UsuariosService(PeluqueriaDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<string?> SubirAvatarAsync(int idUsuario, IFormFile file)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id_Usuario == idUsuario);
            if (usuario == null)
            {
                return null;
            }

            var url = await _cloudinaryService.UploadAvatarAsync(file, idUsuario);
            usuario.Avatar_Url = url;

            await _context.SaveChangesAsync();
            return url;
        }
    }
}
