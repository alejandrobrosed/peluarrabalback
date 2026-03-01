using back.modelos;
using back.dto;
using back.bbdd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;


namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly PeluqueriaDbContext _context;

        public ReservasController(PeluqueriaDbContext context)
        {
            _context = context;
        }

        ///METODO PRIVADO AUXILIAR
        private bool enHorario(TimeOnly inicio, TimeOnly fin)
        {
            var mIni = new TimeOnly(9, 0);
            var mFin = new TimeOnly(14, 0);
            var tIni = new TimeOnly(16, 0);
            var tFin = new TimeOnly(22, 0);

            bool mañana = inicio >= mIni && fin <= mFin;
            bool tarde = inicio >= tIni && fin <= tFin;

            return mañana || tarde;
        }

        static bool Solapan(TimeOnly aIni, TimeOnly aFin, TimeOnly bIni, TimeOnly bFin)
            => aIni < bFin && aFin > bIni;

        // GET: /api/reservas
        [HttpGet]
        public IActionResult GetReservas()
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

            return Ok(reservas);
        }

        // GET: /api/reservas/dashboard
        [HttpGet("dashboard")]
        
        public IActionResult GetDashboard()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            // Semana: de hoy a hoy+6 (simple y suficiente)
            var finSemana = hoy.AddDays(6);

            // Mes actual
            var now = DateTime.Now;
            var inicioMes = new DateOnly(now.Year, now.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddDays(-1);

            // KPIs
            var citasHoy = _context.Reservas.Count(r => r.Fecha == hoy && r.Estado != "cancelada");
            var citasSemana = _context.Reservas.Count(r => r.Fecha >= hoy && r.Fecha <= finSemana && r.Estado != "cancelada");

            // Ajusta esto a tu modelo real:
            // Si no tienes Activo/Rol, lo cambiamos después.
            var clientesActivos = _context.Usuarios.Count(u => u.Rol == "cliente" && u.Activo == true);

            var canceladasMes = _context.Reservas.Count(r => r.Estado == "cancelada" && r.Fecha >= inicioMes && r.Fecha <= finMes);

            // Próximas 5 citas
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

            // Top 5 servicios (cuenta reservas no canceladas)
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

            var dto = new DashboardDto
            {
                CitasHoy = citasHoy,
                CitasSemana = citasSemana,
                ClientesActivos = clientesActivos,
                CanceladasMes = canceladasMes,
                ProximasCitas = proximas,
                TopServicios = topServicios
            };

            return Ok(dto);
        }

        // GET: /api/reservas/cliente/5
        [HttpGet("cliente/{id}")]
        public IActionResult GetReservasCliente(int id)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            var reservas = _context.Reservas
                .Where(r => r.Id_Cliente == id)
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

            return Ok(reservas);
        }

        // GET: /api/reservas/3
        [HttpGet("{id}")]
        public IActionResult GetReserva(int id)
        {
            var reserva = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Servicio)
                .Include(r => r.Empleado)
                .Where(r => r.Id_Reserva == id)
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
                return NotFound();

            return Ok(new
            {
                reserva.Id_Reserva,
                reserva.Fecha,
                reserva.Hora_Inicio,
                reserva.Estado
            });
        }

        //POST: /api/reserva
        [HttpPost]
        public IActionResult CrearReserva([FromBody] ReservaCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                return BadRequest("El cliente ya tiene una cita ese dia");

            var servicio = _context.Servicios.Find(reserva.Id_Servicio);
            if (servicio == null)
                return BadRequest("Servicio no encontrado");

            var inicio = reserva.Hora_Inicio;
            var fin = inicio.AddMinutes(servicio.Duracion_Minutos);

            if (!enHorario(inicio, fin))
                return BadRequest("La reserva está fuera del horario");

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
                return BadRequest("Este empleado ya tiene una cita en esa franja horaria");

            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            return Ok(new
            {
                reserva.Id_Reserva,
                reserva.Fecha,
                reserva.Hora_Inicio,
                reserva.Estado
            });
        }



        //PUT: /api/reservas/5
        [HttpPut("{id}")]
        public IActionResult ActualizarReserva(int id, [FromBody] ReservaUpdateDTO dto)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
                return NotFound();

            reserva.Id_Cliente = dto.Id_Cliente;
            reserva.Id_Servicio = dto.Id_Servicio;
            reserva.Id_Empleado = dto.Id_Empleado;
            reserva.Fecha = dto.Fecha;
            reserva.Estado = dto.Estado;
            reserva.Hora_Inicio = dto.Hora_Inicio;
            reserva.Observaciones = dto.Observaciones;

            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: /api/reserva/5
        [HttpDelete("{id}")]
        public IActionResult EliminarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
            {
                return NotFound();
            }
            _context.Reservas.Remove(reserva);

            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: /api/servicios/5
        [HttpDelete("{id}/cancelar")]
        public IActionResult CancelarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
            {
                return NotFound();
            }
            reserva.Estado = "cancelada";

            _context.SaveChanges();
            return NoContent();
        }


    }
}