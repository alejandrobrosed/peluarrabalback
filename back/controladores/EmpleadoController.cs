using back.modelos;
using back.bbdd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;


namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController: ControllerBase
    {
        private readonly PeluqueriaDbContext _context;

        public EmpleadoController(PeluqueriaDbContext context)
        {
            _context = context;
        }

        // GET: /api/empleados
        [HttpGet]
        public IActionResult GetEmpleados()
        {
            var empleados = _context.Empleados
                .Include(e => e.Usuario)
                .ToList();
                return Ok(empleados);
        }

        // GET: /api/empleados/3
        [HttpGet("{id}")]
        public IActionResult GetReserva(int id)
        {
            var empleado = _context.Empleados
                .Include(e => e.Usuario)
                .FirstOrDefault(e => e.Id_Empleado == id);

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

            _context.Empleados.Add(empleado);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetReserva), new {id = empleado.Id_Empleado}, empleado);
        }

        //DELETE: /api/empleado/5
        [HttpDelete("{id}")]
        public IActionResult EliminarEmpleado(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if(empleado == null)
            {
                return NotFound();
            }
            _context.Empleados.Remove(empleado);

            _context.SaveChanges();
            return NoContent();
        }


    }
}