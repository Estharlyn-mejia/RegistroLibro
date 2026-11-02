
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiante.Services;
using RegistroPrestamos.Data;
using RegistroPrestamos.Services;

namespace RegistroPrestamos.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrestamosController(PrestamosServices prestamosServices)
    : ControllerBase
{
    // GET: api/Prestamos
    [HttpGet]
    public async Task<ActionResult<List<Prestamo>>> Get()
    {
        var resultado = await prestamosServices.ObtenerTodos();
        return Ok(resultado);
    }

    // GET: api/Prestamos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Prestamo>> Get(int id)
    {
        if (id <= 0)
            return BadRequest("El ID debe ser mayor que cero.");

        var prestamo = await prestamosServices.Buscar(id);

        if (prestamo is null)
            return NotFound("No se encontró el préstamo.");

        return Ok(prestamo);
    }

    // POST: api/Prestamos
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Prestamo prestamo)
    {
        if (prestamo is null)
            return BadRequest("Debe proporcionar los datos del préstamo.");

        var resultado = await prestamosServices.Guardar(prestamo);

        if (!resultado)
            return BadRequest("No se pudo guardar el préstamo. Verifique los datos.");

        return Ok("Préstamo registrado correctamente.");
    }

    // PUT: api/Prestamos/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(
        int id,
        [FromBody] Prestamo prestamo)
    {
        if (prestamo is null)
            return BadRequest("Debe proporcionar los datos del préstamo.");

        if (id <= 0 || id != prestamo.PrestamoId)
            return BadRequest("El ID de la ruta no coincide con el préstamo.");

        var resultado = await prestamosServices.Modificar(prestamo);

        if (!resultado)
            return BadRequest("No se pudo modificar el préstamo.");

        return Ok("Préstamo actualizado correctamente.");
    }

    // PUT: api/Prestamos/5/devolucion
    [HttpPut("{id:int}/devolucion")]
    public async Task<IActionResult> RegistrarDevolucion(int id)
    {
        if (id <= 0)
            return BadRequest("El ID debe ser mayor que cero.");

        var resultado =
            await prestamosServices.RegistrarDevolucion(id);

        if (!resultado)
            return BadRequest("No se pudo registrar la devolución.");

        return Ok("Devolución registrada correctamente.");
    }

    // DELETE: api/Prestamos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("El ID debe ser mayor que cero.");

        var resultado = await prestamosServices.Eliminar(id);

        if (!resultado)
            return NotFound("No se pudo eliminar el préstamo.");

        return Ok("Préstamo eliminado correctamente.");
    }
}
