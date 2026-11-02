using Aplicada1.Core;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiante.Services;
using RegistroPrestamos.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistroPrestamos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController(EstudiantesServices estudiantesServices) : ControllerBase
    {
        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<Result<List<Estudiante>>>> Get()
        {
            var resultado = await estudiantesServices.GetList(e => true);
            return Ok(resultado);
        }

        // GET api/Estudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<Estudiante?>>> Get(int id)
        {
            var resultado = await estudiantesServices.Buscar(id);
            return Ok(resultado);
        }

        // POST api/Estudiantes
        [HttpPost]
        public async Task<ActionResult<Result>> Post([FromBody] Estudiante estudiante)
        {
            var resultado = await estudiantesServices.Guardar(estudiante);
            return Ok(resultado);
        }

        // PUT api/Estudiantes/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result>> Put(int id, [FromBody] Estudiante estudiante)
        {
            if(id != estudiante.EstudianteId)
                return BadRequest("El ID de la ruta no coincide con el ID del estudiante.");

            var resultado = await estudiantesServices.Modificar(estudiante);
            return Ok(resultado);
        }

        // DELETE api/Estudiantes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var resultado = await estudiantesServices.Eliminar(id);
            return Ok(resultado);
        }
    }
}
