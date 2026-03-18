using back.modelos;
using Microsoft.AspNetCore.Mvc;
using back.servicios;

namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController: ControllerBase
    {
        private readonly ServiciosService _serviciosService;

        public ServiciosController(ServiciosService serviciosService)
        {
            _serviciosService = serviciosService;
        }

        // GET: /api/servicios
        [HttpGet]
        public IActionResult GetServicios(bool?activo, int page=1, int pageSize=5)
        {
            var (total, servicios) = _serviciosService.GetServicios(activo, page, pageSize);
            return Ok(new
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Data = servicios
            });
        }

        // GET: /api/servicios?activo=true
        [HttpGet("activos")]
        public IActionResult GetServiciosActivos([FromBody] bool? activo)
        {
            var servicios = _serviciosService.GetServiciosActivos(activo);
            return Ok(servicios);
        }

        // GET: /api/servicios/3
        [HttpGet("{id}")]
        public IActionResult GetServicio(int id)
        {
            var servicio = _serviciosService.GetServicioById(id);
            if(servicio == null)
            {
                return NotFound();
            }
            return Ok(servicio);
        }

        //POST: /api/servicios
        [HttpPost]
        public IActionResult CrearServicio([FromBody] Servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _serviciosService.CrearServicio(servicio);
            return Ok(servicio);
        }

        //PUT: /api/servicios/5
        [HttpPut("{id}")]
        public IActionResult ActualizarServicio(int id, [FromBody] Servicio servicioActualizado)
        {
            var actualizado = _serviciosService.ActualizarServicio(id, servicioActualizado);
            if (!actualizado)
            {
                return NotFound();
            }
            return NoContent();
        }

        //DELETE: /api/servicios/5
        [HttpDelete("{id}")]
        public IActionResult EliminarServicio(int id)
        {
            var eliminado = _serviciosService.EliminarServicio(id);
            if (!eliminado)
            {
                return NotFound();
            }
            return NoContent();
        }

        //DELETE: /api/servicios/5
        [HttpDelete("{id}/inactivo")]
        public IActionResult MarcarInactivoServicio(int id)
        {
            var marcado = _serviciosService.MarcarInactivoServicio(id);
            if (!marcado)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}