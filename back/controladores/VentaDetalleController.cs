using back.modelos;
using back.bbdd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;


namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaDetalleController: ControllerBase
    {
        private readonly PeluqueriaDbContext _context;

        public VentaDetalleController(PeluqueriaDbContext context)
        {
            _context = context;
        }

        // GET: /api/detalles
        [HttpGet]
        public IActionResult GetDetalles()
        {
            var detalles = _context.VentaDetalles
                .Include(r => r.Producto)
                .Include(r => r.Venta)
                .ToList();
                return Ok(detalles);
        }

        // GET: /api/detalles/3
        [HttpGet("{id}")]
        public IActionResult GetDetalles(int id)
        {
            var detalles = _context.VentaDetalles
                .Include(r => r.Producto)
                .Include(r => r.Venta)
                .FirstOrDefault(v => v.Id_Detalle == id);

            if(detalles == null)
            {
                return NotFound();
            }
            return Ok(detalles);
        }

        // GET: /api/detalles/venta/3
        [HttpGet("/venta/{id}")]
        public IActionResult GetDetallesPorVenta(int id)
        {
            var detalles = _context.VentaDetalles
                .Where(v => v.Id_Venta == id)
                .Include(v => v.Producto)
                .ToList();

            return Ok(detalles);
        }

        //POST: /api/ventas
        [HttpPost]
        public IActionResult CrearVenta([FromBody] VentaDetalle detalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(detalle.Cantidad <= 0)
            {
                return BadRequest("La cantidad debe ser mayor que cero");
            }

            _context.VentaDetalles.Add(detalle);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDetalles), new {id = detalle.Id_Detalle}, detalle);
        }

        //PUT: /api/detalles/5
        [HttpPut("{id}")]
        public IActionResult ActualizarDetalle(int id, [FromBody] VentaDetalle detalleActualizado)
        {
            if(id != detalleActualizado.Id_Detalle)
            {
                return BadRequest();
            }
            var detalle = _context.VentaDetalles.Find(id);
            if(detalle == null)
            {
                return NotFound();
            }
            if(detalle.Cantidad <= 0)
            {
                return BadRequest("La cantidad debe ser mayor que cero");
            }
            detalle.Cantidad = detalleActualizado.Cantidad;
            detalle.PrecioUnitario = detalleActualizado.PrecioUnitario;

            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: /api/detalles/5
        [HttpDelete("{id}")]
        public IActionResult EliminarDetalles(int id)
        {
            var detalle = _context.VentaDetalles.Find(id);
            if(detalle == null)
            {
                return NotFound();
            }
            _context.VentaDetalles.Remove(detalle);

            _context.SaveChanges();
            return NoContent();
        }


    }
}