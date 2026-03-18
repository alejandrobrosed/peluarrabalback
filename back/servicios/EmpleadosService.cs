using back.bbdd;
using back.modelos;
using Microsoft.EntityFrameworkCore;

namespace back.servicios
{
    public class EmpleadosService
    {
        private readonly PeluqueriaDbContext _context;

        public EmpleadosService(PeluqueriaDbContext context)
        {
            _context = context;
        }

        public List<Empleado> GetEmpleados()
        {
            return _context.Empleados
                .Include(e => e.Usuario)
                .ToList();
        }

        public Empleado? GetEmpleadoById(int id)
        {
            return _context.Empleados
                .Include(e => e.Usuario)
                .FirstOrDefault(e => e.Id_Empleado == id);
        }

        public Empleado CrearEmpleado(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            _context.SaveChanges();
            return empleado;
        }

        public bool EliminarEmpleado(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado == null)
            {
                return false;
            }

            _context.Empleados.Remove(empleado);
            _context.SaveChanges();
            return true;
        }
    }
}
