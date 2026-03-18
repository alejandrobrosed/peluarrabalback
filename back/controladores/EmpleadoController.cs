using back.modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using back.servicios;

namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController: ControllerBase
    {
        private readonly EmpleadosService _empleadosService;

        public EmpleadoController(EmpleadosService empleadosService)
        {
            _empleadosService = empleadosService;
        }

        // GET: /api/empleados
        [HttpGet]
        public IActionResult GetEmpleados()
        {
            var empleados = _empleadosService.GetEmpleados();
            return Ok(empleados);
        }

        // GET: /api/empleados/3
        [HttpGet("{id}")]
        public IActionResult GetReserva(int id)
        {
            var empleado = _empleadosService.GetEmpleadoById(id);

            if(empleado == null)
            {
                return NotFound();
            }
            return Ok(empleado);
        }

        //POST: /api/empleado
        [HttpPost]
        public IActionResult CrearEmpleado([FromBody] Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _empleadosService.CrearEmpleado(empleado);
            return CreatedAtAction(nameof(GetReserva), new {id = empleado.Id_Empleado}, empleado);
        }

        //DELETE: /api/empleado/5
        [HttpDelete("{id}")]
        public IActionResult EliminarEmpleado(int id)
        {
            var eliminado = _empleadosService.EliminarEmpleado(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}