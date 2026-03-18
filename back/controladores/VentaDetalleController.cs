using back.modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using back.servicios;

namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaDetalleController: ControllerBase
    {
        private readonly VentaDetallesService _ventaDetallesService;

        public VentaDetalleController(VentaDetallesService ventaDetallesService)
        {
            _ventaDetallesService = ventaDetallesService;
        }

        // GET: /api/detalles
        [HttpGet]
        public IActionResult GetDetalles()
        {
            var detalles = _ventaDetallesService.GetDetalles();
            return Ok(detalles);
        }

        // GET: /api/detalles/3
        [HttpGet("{id}")]
        public IActionResult GetDetalles(int id)
        {
            var detalles = _ventaDetallesService.GetDetalleById(id);

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
            var detalles = _ventaDetallesService.GetDetallesPorVenta(id);
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

            _ventaDetallesService.CrearDetalle(detalle);
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

            if(detalleActualizado.Cantidad <= 0)
            {
                return BadRequest("La cantidad debe ser mayor que cero");
            }

            var actualizado = _ventaDetallesService.ActualizarDetalle(id, detalleActualizado);
            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        //DELETE: /api/detalles/5
        [HttpDelete("{id}")]
        public IActionResult EliminarDetalles(int id)
        {
            var eliminado = _ventaDetallesService.EliminarDetalle(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}