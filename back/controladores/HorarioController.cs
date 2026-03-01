using back.modelos;
using back.bbdd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;


namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioController: ControllerBase
    {
        private readonly PeluqueriaDbContext _context;

        public HorarioController(PeluqueriaDbContext context)
        {
            _context = context;
        }

        // GET: /api/horarios
        [HttpGet]
        public IActionResult GetHorarios()
        {
            var horarios = _context.Horarios
                .Include(r => r.Empleado)
                .ToList();
                return Ok(horarios);
        }

        // GET: /api/horarios/3
        [HttpGet("{id}")]
        public IActionResult GetHorarios(int id)
        {
            var horarios = _context.Horarios
                .Include(r => r.Empleado)
                .FirstOrDefault(r => r.Id_Horario == id);

            if(horarios == null)
            {
                return NotFound();
            }
            return Ok(horarios);
        }


        // GET: /api/horarios/empleado/2
        [HttpGet("empleado/{id}")]
        public IActionResult GetHorariosPorEmpleado(int id)
        {
            var horarios = _context.Horarios
                .Include(r => r.Empleado)
                .FirstOrDefault(r => r.Id_Empleado == id);

            if(horarios == null)
            {
                return NotFound();
            }
            return Ok(horarios);
        }


        //POST: /api/horario
        [HttpPost]
        public IActionResult CrearHorario([FromBody] Horario horario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if(horario.Hora_Fin <= horario.Hora_Inicio)
            {
                return BadRequest("La hora de fin debe ser mauor que la hora de inicio");
            }

            _context.Horarios.Add(horario);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetHorarios), new {id = horario.Id_Horario}, horario);
        }

        //PUT: /api/horario/5
        [HttpPut("{id}")]
        public IActionResult ActualizarHorario(int id, [FromBody] Horario horarioActualizado)
        {
            var horario = _context.Horarios.Find(id);
            if(horario == null)
            {
                return NotFound();
            }

            if(horario.Hora_Fin <= horario.Hora_Inicio)
            {
                return BadRequest("La hora de fin debe ser mauor que la hora de inicio");
            }


            horario.Dia_Semana = horarioActualizado.Dia_Semana;
            horario.Hora_Inicio = horarioActualizado.Hora_Inicio;
            horario.Hora_Fin = horarioActualizado.Hora_Fin;
            horario.Id_Empleado = horarioActualizado.Id_Empleado;

            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: /api/horario/5
        [HttpDelete("{id}")]
        public IActionResult EliminarHorario(int id)
        {
            var horario = _context.Horarios.Find(id);
            if(horario == null)
            {
                return NotFound();
            }
            _context.Horarios.Remove(horario);

            _context.SaveChanges();
            return NoContent();
        }


    }
}