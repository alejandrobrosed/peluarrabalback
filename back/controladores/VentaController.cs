using back.modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using back.servicios;

namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController: ControllerBase
    {
        private readonly VentasService _ventasService;

        public VentaController(VentasService ventasService)
        {
            _ventasService = ventasService;
        }

        // GET: /api/ventas
        [HttpGet]
        public IActionResult GetVentas()
        {
            var ventas = _ventasService.GetVentas();
            return Ok(ventas);
        }

        // GET: /api/ventas/3
        [HttpGet("{id}")]
        public IActionResult GetVentas(int id)
        {
            var ventas = _ventasService.GetVentaById(id);

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
            var ventas = _ventasService.GetVentasPorCliente(id);
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

            _ventasService.CrearVenta(venta);
            return CreatedAtAction(nameof(GetVentas), new {id = venta.Id_Venta}, venta);
        }

        //PUT: /api/ventas/5
        [HttpPut("{id}")]
        public IActionResult ActualizarVenta(int id, [FromBody] Venta ventaActualizado)
        {
            var actualizado = _ventasService.ActualizarVenta(id, ventaActualizado);
            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        //DELETE: /api/venta/5
        [HttpDelete("{id}")]
        public IActionResult EliminarVenta(int id)
        {
            var eliminado = _ventasService.EliminarVenta(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}