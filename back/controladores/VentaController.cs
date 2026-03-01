using back.modelos;
using back.bbdd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;


namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController: ControllerBase
    {
        private readonly PeluqueriaDbContext _context;

        public VentaController(PeluqueriaDbContext context)
        {
            _context = context;
        }

        // GET: /api/ventas
        [HttpGet]
        public IActionResult GetVentas()
        {
            var ventas = _context.Ventas
                .Include(r => r.Cliente)
                .ToList();
                return Ok(ventas);
        }

        // GET: /api/ventas/3
        [HttpGet("{id}")]
        public IActionResult GetVentas(int id)
        {
            var ventas = _context.Ventas
                .Include(v => v.Cliente)
                .FirstOrDefault(v => v.Id_Cliente == id);

            if(ventas == null)
            {
                return NotFound();
            }
            return Ok(ventas);
        }

        // GET: /api/ventas/cliente/3
        [HttpGet("/cliente/{id}")]
        public IActionResult GetVentasPorCliente(int id)
        {
            var ventas = _context.Ventas
                .Where(v => v.Id_Cliente == id)
                .ToList();

            return Ok(ventas);
        }

        //POST: /api/ventas
        [HttpPost]
        public IActionResult CrearVenta([FromBody] Venta venta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            venta.Fecha = DateTime.Now;

            _context.Ventas.Add(venta);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetVentas), new {id = venta.Id_Venta}, venta);
        }

        //PUT: /api/ventas/5
        [HttpPut("{id}")]
        public IActionResult ActualizarVenta(int id, [FromBody] Venta ventaActualizado)
        {
            var venta = _context.Ventas.Find(id);
            if(venta == null)
            {
                return NotFound();
            }
            venta.Total = ventaActualizado.Total;

            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: /api/venta/5
        [HttpDelete("{id}")]
        public IActionResult EliminarVenta(int id)
        {
            var venta = _context.Ventas.Find(id);
            if(venta == null)
            {
                return NotFound();
            }
            _context.Ventas.Remove(venta);

            _context.SaveChanges();
            return NoContent();
        }


    }
}