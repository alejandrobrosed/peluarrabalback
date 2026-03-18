using back.modelos;
using back.dto;
using back.bbdd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using back.servicios;

namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly PeluqueriaDbContext _context;
        private readonly ReservasService _reservasService;

        public ReservasController(PeluqueriaDbContext context, ReservasService reservasService)
        {
            _context = context;
            _reservasService = reservasService;
        }

        // GET: /api/reservas
        [HttpGet]
        public IActionResult GetReservas()
        {
            return Ok(_reservasService.GetReservasListado());
        }

        // GET: /api/reservas/dashboard
        [HttpGet("dashboard")]
        
        public IActionResult GetDashboard()
        {
            return Ok(_reservasService.GetDashboard());
        }

        // GET: /api/reservas/cliente/5
        [HttpGet("cliente/{id}")]
        public IActionResult GetReservasCliente(int id)
        {
            return Ok(_reservasService.GetReservasCliente(id));
        }

        // GET: /api/reservas/3
        [HttpGet("{id}")]
        public IActionResult GetReserva(int id)
        {
            var reserva = _reservasService.GetReserva(id);
            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }

        //POST: /api/reserva
        [HttpPost]
        public IActionResult CrearReserva([FromBody] ReservaCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _reservasService.CrearReserva(dto);
            if (!result.ok)
            {
                return BadRequest(result.error);
            }

            return Ok(result.data);
        }



        //PUT: /api/reservas/5
        [HttpPut("{id}")]
        public IActionResult ActualizarReserva(int id, [FromBody] ReservaUpdateDTO dto)
        {
            var ok = _reservasService.ActualizarReserva(id, dto);
            if (!ok)
            {
                return NotFound();
            }

            return NoContent();
        }

        //DELETE: /api/reserva/5
        [HttpDelete("{id}")]
        public IActionResult EliminarReserva(int id)
        {
            var ok = _reservasService.EliminarReserva(id);
            if (!ok)
            {
                return NotFound();
            }

            return NoContent();
        }

        //DELETE: /api/servicios/5
        [HttpDelete("{id}/cancelar")]
        public IActionResult CancelarReserva(int id)
        {
            var ok = _reservasService.CancelarReserva(id);
            if (!ok)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}