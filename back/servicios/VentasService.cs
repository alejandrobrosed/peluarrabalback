using back.bbdd;
using back.modelos;
using Microsoft.EntityFrameworkCore;

namespace back.servicios
{
    public class VentasService
    {
        private readonly PeluqueriaDbContext _context;

        public VentasService(PeluqueriaDbContext context)
        {
            _context = context;
        }

        public List<Venta> GetVentas()
        {
            return _context.Ventas
                .Include(v => v.Cliente)
                .ToList();
        }

        public Venta? GetVentaById(int id)
        {
            return _context.Ventas
                .Include(v => v.Cliente)
                .FirstOrDefault(v => v.Id_Venta == id);
        }

        public List<Venta> GetVentasPorCliente(int idCliente)
        {
            return _context.Ventas
                .Where(v => v.Id_Cliente == idCliente)
                .ToList();
        }

        public Venta CrearVenta(Venta venta)
        {
            venta.Fecha = DateTime.Now;
            _context.Ventas.Add(venta);
            _context.SaveChanges();
            return venta;
        }

        public bool ActualizarVenta(int id, Venta ventaActualizado)
        {
            var venta = _context.Ventas.Find(id);
            if (venta == null)
            {
                return false;
            }

            venta.Total = ventaActualizado.Total;
            _context.SaveChanges();
            return true;
        }

        public bool EliminarVenta(int id)
        {
            var venta = _context.Ventas.Find(id);
            if (venta == null)
            {
                return false;
            }

            _context.Ventas.Remove(venta);
            _context.SaveChanges();
            return true;
        }
    }
}
