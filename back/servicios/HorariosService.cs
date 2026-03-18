using back.bbdd;
using back.modelos;
using Microsoft.EntityFrameworkCore;

namespace back.servicios
{
    public class HorariosService
    {
        private readonly PeluqueriaDbContext _context;

        public HorariosService(PeluqueriaDbContext context)
        {
            _context = context;
        }

        public List<Horario> GetHorarios()
        {
            return _context.Horarios
                .Include(h => h.Empleado)
                .ToList();
        }

        public Horario? GetHorarioById(int id)
        {
            return _context.Horarios
                .Include(h => h.Empleado)
                .FirstOrDefault(h => h.Id_Horario == id);
        }

        public Horario? GetHorarioPorEmpleado(int idEmpleado)
        {
            return _context.Horarios
                .Include(h => h.Empleado)
                .FirstOrDefault(h => h.Id_Empleado == idEmpleado);
        }

        public Horario CrearHorario(Horario horario)
        {
            _context.Horarios.Add(horario);
            _context.SaveChanges();
            return horario;
        }

        public bool ActualizarHorario(int id, Horario horarioActualizado)
        {
            var horario = _context.Horarios.Find(id);
            if (horario == null)
            {
                return false;
            }

            horario.Dia_Semana = horarioActualizado.Dia_Semana;
            horario.Hora_Inicio = horarioActualizado.Hora_Inicio;
            horario.Hora_Fin = horarioActualizado.Hora_Fin;
            horario.Id_Empleado = horarioActualizado.Id_Empleado;

            _context.SaveChanges();
            return true;
        }

        public bool EliminarHorario(int id)
        {
            var horario = _context.Horarios.Find(id);
            if (horario == null)
            {
                return false;
            }

            _context.Horarios.Remove(horario);
            _context.SaveChanges();
            return true;
        }
    }
}
