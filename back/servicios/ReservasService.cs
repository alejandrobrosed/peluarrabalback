using back.bbdd;
using back.dto;
using back.modelos;
using Microsoft.EntityFrameworkCore;

namespace back.servicios
{
    public class ReservasService
    {
        private readonly PeluqueriaDbContext _context;

        public ReservasService(PeluqueriaDbContext context)
        {
            _context = context;
        }

        private bool EnHorario(TimeOnly inicio, TimeOnly fin)
        {
            var mIni = new TimeOnly(9, 0);
            var mFin = new TimeOnly(14, 0);
            var tIni = new TimeOnly(16, 0);
            var tFin = new TimeOnly(22, 0);

            bool manana = inicio >= mIni && fin <= mFin;
            bool tarde = inicio >= tIni && fin <= tFin;

            return manana || tarde;
        }

        private static bool Solapan(TimeOnly aIni, TimeOnly aFin, TimeOnly bIni, TimeOnly bFin)
            => aIni < bFin && aFin > bIni;

        public List<object> GetReservasListado()
        {
            var reservas = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Servicio)
                .Include(r => r.Empleado)
                .Select(r => new
                {
                    r.Id_Reserva,
                    r.Fecha,
                    r.Hora_Inicio,
                    r.Estado,
                    Cliente = new
                    {
                        r.Cliente.Id_Usuario,
                        r.Cliente.Nombre
                    },
                    Servicio = new
                    {
                        r.Servicio.Id_Servicio,
                        r.Servicio.Nombre
                    },
                    Empleado = new
                    {
                        r.Empleado.Id_Empleado,
                        r.Empleado.Especialidad
                    }
                })
                .ToList();

            return reservas.Cast<object>().ToList();
        }

        public DashboardDto GetDashboard()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var finSemana = hoy.AddDays(6);

            var now = DateTime.Now;
            var inicioMes = new DateOnly(now.Year, now.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddDays(-1);

            var citasHoy = _context.Reservas.Count(r => r.Fecha == hoy && r.Estado != "cancelada");
            var citasSemana = _context.Reservas.Count(r => r.Fecha >= hoy && r.Fecha <= finSemana && r.Estado != "cancelada");
            var clientesActivos = _context.Usuarios.Count(u => u.Rol == "cliente" && u.Activo == true);
            var canceladasMes = _context.Reservas.Count(r => r.Estado == "cancelada" && r.Fecha >= inicioMes && r.Fecha <= finMes);

            var proximas = _context.Reservas
                .Where(r => r.Estado != "cancelada" && r.Fecha >= hoy)
                .OrderBy(r => r.Fecha)
                .ThenBy(r => r.Hora_Inicio)
                .Select(r => new ReservaMiniDto
                {
                    Id_Reserva = r.Id_Reserva,
                    Fecha = r.Fecha,
                    Hora_Inicio = r.Hora_Inicio,
                    Estado = r.Estado,
                    ClienteNombre = r.Cliente.Nombre,
                    ServicioNombre = r.Servicio.Nombre,
                    EmpleadoEspecialidad = r.Empleado.Especialidad
                })
                .Take(5)
                .ToList();

            var topServicios = _context.Reservas
                .Where(r => r.Estado != "cancelada")
                .GroupBy(r => new { r.Id_Servicio, r.Servicio.Nombre })
                .Select(g => new TopServicioDto
                {
                    Id_Servicio = g.Key.Id_Servicio,
                    Nombre = g.Key.Nombre,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .ToList();

            return new DashboardDto
            {
                CitasHoy = citasHoy,
                CitasSemana = citasSemana,
                ClientesActivos = clientesActivos,
                CanceladasMes = canceladasMes,
                ProximasCitas = proximas,
                TopServicios = topServicios
            };
        }

        public List<object> GetReservasCliente(int idCliente)
        {
            var reservas = _context.Reservas
                .Where(r => r.Id_Cliente == idCliente)
                .OrderByDescending(r => r.Fecha)
                .ThenByDescending(r => r.Hora_Inicio)
                .Select(r => new
                {
                    r.Id_Reserva,
                    r.Fecha,
                    r.Hora_Inicio,
                    r.Estado,
                    Cliente = r.Cliente.Nombre,
                    Servicio = r.Servicio.Nombre,
                    Empleado = r.Empleado.Especialidad
                })
                .ToList();

            return reservas.Cast<object>().ToList();
        }

        public object? GetReserva(int idReserva)
        {
            var reserva = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Servicio)
                .Include(r => r.Empleado)
                .Where(r => r.Id_Reserva == idReserva)
                .Select(r => new
                {
                    r.Id_Reserva,
                    r.Fecha,
                    r.Hora_Inicio,
                    r.Estado,
                    r.Observaciones,
                    Cliente = new
                    {
                        r.Cliente.Id_Usuario,
                        r.Cliente.Nombre
                    },
                    Servicio = new
                    {
                        r.Servicio.Id_Servicio,
                        r.Servicio.Nombre
                    },
                    Empleado = new
                    {
                        r.Empleado.Id_Empleado,
                        r.Empleado.Especialidad
                    }
                })
                .FirstOrDefault();

            if (reserva == null)
            {
                return null;
            }

            return new
            {
                reserva.Id_Reserva,
                reserva.Fecha,
                reserva.Hora_Inicio,
                reserva.Estado
            };
        }

        public (bool ok, string? error, object? data) CrearReserva(ReservaCreateDTO dto)
        {
            var reserva = new Reserva
            {
                Id_Cliente = dto.Id_Cliente,
                Id_Servicio = dto.Id_Servicio,
                Id_Empleado = dto.Id_Empleado,
                Fecha = dto.Fecha,
                Hora_Inicio = dto.Hora_Inicio,
                Observaciones = dto.Observaciones,
                Estado = "pendiente"
            };

            var tiene = _context.Reservas.Any(r =>
                r.Id_Cliente == reserva.Id_Cliente &&
                r.Fecha == reserva.Fecha &&
                r.Estado != "cancelada");

            if (tiene)
            {
                return (false, "El cliente ya tiene una cita ese dia", null);
            }

            var servicio = _context.Servicios.Find(reserva.Id_Servicio);
            if (servicio == null)
            {
                return (false, "Servicio no encontrado", null);
            }

            var inicio = reserva.Hora_Inicio;
            var fin = inicio.AddMinutes(servicio.Duracion_Minutos);

            if (!EnHorario(inicio, fin))
            {
                return (false, "La reserva está fuera del horario", null);
            }

            var empleadoOcupado = _context.Reservas.Where(r =>
                    r.Id_Empleado == reserva.Id_Empleado &&
                    r.Fecha == reserva.Fecha &&
                    r.Estado != "cancelada")
                .AsEnumerable()
                .Any(r =>
                {
                    var s = _context.Servicios.Find(r.Id_Servicio);
                    if (s == null) return false;
                    var rIni = r.Hora_Inicio;
                    var rFin = rIni.AddMinutes(s.Duracion_Minutos);
                    return Solapan(inicio, fin, rIni, rFin);
                });

            if (empleadoOcupado)
            {
                return (false, "Este empleado ya tiene una cita en esa franja horaria", null);
            }

            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            return (true, null, new
            {
                reserva.Id_Reserva,
                reserva.Fecha,
                reserva.Hora_Inicio,
                reserva.Estado
            });
        }

        public bool ActualizarReserva(int idReserva, ReservaUpdateDTO dto)
        {
            var reserva = _context.Reservas.Find(idReserva);
            if (reserva == null)
            {
                return false;
            }

            reserva.Id_Cliente = dto.Id_Cliente;
            reserva.Id_Servicio = dto.Id_Servicio;
            reserva.Id_Empleado = dto.Id_Empleado;
            reserva.Fecha = dto.Fecha;
            reserva.Estado = dto.Estado;
            reserva.Hora_Inicio = dto.Hora_Inicio;
            reserva.Observaciones = dto.Observaciones;

            _context.SaveChanges();
            return true;
        }

        public bool EliminarReserva(int idReserva)
        {
            var reserva = _context.Reservas.Find(idReserva);
            if (reserva == null)
            {
                return false;
            }

            _context.Reservas.Remove(reserva);
            _context.SaveChanges();
            return true;
        }

        public bool CancelarReserva(int idReserva)
        {
            var reserva = _context.Reservas.Find(idReserva);
            if (reserva == null)
            {
                return false;
            }

            reserva.Estado = "cancelada";
            _context.SaveChanges();
            return true;
        }
    }
}
