using back.modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using back.servicios;

namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioController: ControllerBase
    {
        private readonly HorariosService _horariosService;

        public HorarioController(HorariosService horariosService)
        {
            _horariosService = horariosService;
        }

        // GET: /api/horarios
        [HttpGet]
        public IActionResult GetHorarios()
        {
            var horarios = _horariosService.GetHorarios();
            return Ok(horarios);
        }

        // GET: /api/horarios/3
        [HttpGet("{id}")]
        public IActionResult GetHorarios(int id)
        {
            var horarios = _horariosService.GetHorarioById(id);

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
            var horarios = _horariosService.GetHorarioPorEmpleado(id);

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
                return BadRequest("La hora de fin debe ser mayor que la hora de inicio");
            }

            _horariosService.CrearHorario(horario);
            return CreatedAtAction(nameof(GetHorarios), new {id = horario.Id_Horario}, horario);
        }

        //PUT: /api/horario/5
        [HttpPut("{id}")]
        public IActionResult ActualizarHorario(int id, [FromBody] Horario horarioActualizado)
        {
            if(horarioActualizado.Hora_Fin <= horarioActualizado.Hora_Inicio)
            {
                return BadRequest("La hora de fin debe ser mayor que la hora de inicio");
            }

            var actualizado = _horariosService.ActualizarHorario(id, horarioActualizado);
            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        //DELETE: /api/horario/5
        [HttpDelete("{id}")]
        public IActionResult EliminarHorario(int id)
        {
            var eliminado = _horariosService.EliminarHorario(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}