using back.bbdd;
using back.modelos;
using Microsoft.EntityFrameworkCore;

namespace back.servicios
{
    public class VentaDetallesService
    {
        private readonly PeluqueriaDbContext _context;

        public VentaDetallesService(PeluqueriaDbContext context)
        {
            _context = context;
        }

        public List<VentaDetalle> GetDetalles()
        {
            return _context.VentaDetalles
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .ToList();
        }

        public VentaDetalle? GetDetalleById(int id)
        {
            return _context.VentaDetalles
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .FirstOrDefault(d => d.Id_Detalle == id);
        }

        public List<VentaDetalle> GetDetallesPorVenta(int idVenta)
        {
            return _context.VentaDetalles
                .Where(d => d.Id_Venta == idVenta)
                .Include(d => d.Producto)
                .ToList();
        }

        public VentaDetalle CrearDetalle(VentaDetalle detalle)
        {
            _context.VentaDetalles.Add(detalle);
            _context.SaveChanges();
            return detalle;
        }

        public bool ActualizarDetalle(int id, VentaDetalle detalleActualizado)
        {
            var detalle = _context.VentaDetalles.Find(id);
            if (detalle == null)
            {
                return false;
            }

            detalle.Cantidad = detalleActualizado.Cantidad;
            detalle.PrecioUnitario = detalleActualizado.PrecioUnitario;
            _context.SaveChanges();
            return true;
        }

        public bool EliminarDetalle(int id)
        {
            var detalle = _context.VentaDetalles.Find(id);
            if (detalle == null)
            {
                return false;
            }

            _context.VentaDetalles.Remove(detalle);
            _context.SaveChanges();
            return true;
        }
    }
}
