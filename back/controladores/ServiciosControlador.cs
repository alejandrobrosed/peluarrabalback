using back.modelos;
using back.bbdd;
using Microsoft.AspNetCore.Mvc;


namespace back.controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController: ControllerBase
    {
        private readonly PeluqueriaDbContext _context;

        public ServiciosController(PeluqueriaDbContext context)
        {
            _context = context;
        }

        // GET: /api/servicios
        [HttpGet]
        public IActionResult GetServicios(bool?activo, int page=1, int pageSize=5)
        {
            var query = _context.Servicios.Where(s => s.Activo == true).AsQueryable();
            if (activo.HasValue)
            {
                query = query.Where(s => s.Activo == activo.Value);
            }

            var total = query.Count();
            var servicios = query.Skip((page-1) * pageSize).Take(pageSize).ToList();
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
            var query = _context.Servicios.AsQueryable();
            if (activo.HasValue)
            {
                query = query.Where(s => s.Activo == activo.Value);
            }
            var servicios = query.ToList();
            return Ok(servicios);
        }


        // GET: /api/servicios/3
        [HttpGet("{id}")]
        public IActionResult GetServicio(int id)
        {
            var servicio = _context.Servicios.Find(id);
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
            _context.Servicios.Add(servicio);
            _context.SaveChanges();
            return Ok(servicio);
        }

        //PUT: /api/servicios/5
        [HttpPut("{id}")]
        public IActionResult ActualizarServicio(int id, [FromBody] Servicio servicioActualizado)
        {
            var servicio = _context.Servicios.Find(id);
            if(servicio == null)
            {
                return NotFound();
            }
            servicio.Nombre = servicioActualizado.Nombre;
            servicio.Duracion_Minutos = servicioActualizado.Duracion_Minutos;
            servicio.Descripcion = servicioActualizado.Descripcion;
            servicio.Precio = servicioActualizado.Precio;
            servicio.Activo = servicioActualizado.Activo;

            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: /api/servicios/5
        [HttpDelete("{id}")]
        public IActionResult EliminarServicio(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if(servicio == null)
            {
                return NotFound();
            }
            _context.Servicios.Remove(servicio);

            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: /api/servicios/5
        [HttpDelete("{id}/inactivo")]
        public IActionResult MarcarInactivoServicio(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if(servicio == null)
            {
                return NotFound();
            }
            servicio.Activo = false;

            _context.SaveChanges();
            return NoContent();
        }


    }
}