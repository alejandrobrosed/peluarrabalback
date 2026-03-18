using back.bbdd;
using back.modelos;

namespace back.servicios
{
    public class ServiciosService
    {
        private readonly PeluqueriaDbContext _context;

        public ServiciosService(PeluqueriaDbContext context)
        {
            _context = context;
        }

        public (int Total, List<Servicio> Data) GetServicios(bool? activo, int page, int pageSize)
        {
            var query = _context.Servicios.Where(s => s.Activo == true).AsQueryable();
            if (activo.HasValue)
            {
                query = query.Where(s => s.Activo == activo.Value);
            }

            var total = query.Count();
            var servicios = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return (total, servicios);
        }

        public List<Servicio> GetServiciosActivos(bool? activo)
        {
            var query = _context.Servicios.AsQueryable();
            if (activo.HasValue)
            {
                query = query.Where(s => s.Activo == activo.Value);
            }

            return query.ToList();
        }

        public Servicio? GetServicioById(int id)
        {
            return _context.Servicios.Find(id);
        }

        public Servicio CrearServicio(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            _context.SaveChanges();
            return servicio;
        }

        public bool ActualizarServicio(int id, Servicio servicioActualizado)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio == null)
            {
                return false;
            }

            servicio.Nombre = servicioActualizado.Nombre;
            servicio.Duracion_Minutos = servicioActualizado.Duracion_Minutos;
            servicio.Descripcion = servicioActualizado.Descripcion;
            servicio.Precio = servicioActualizado.Precio;
            servicio.Activo = servicioActualizado.Activo;

            _context.SaveChanges();
            return true;
        }

        public bool EliminarServicio(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio == null)
            {
                return false;
            }

            _context.Servicios.Remove(servicio);
            _context.SaveChanges();
            return true;
        }

        public bool MarcarInactivoServicio(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio == null)
            {
                return false;
            }

            servicio.Activo = false;
            _context.SaveChanges();
            return true;
        }
    }
}
