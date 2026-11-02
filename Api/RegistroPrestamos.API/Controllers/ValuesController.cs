using Microsoft.AspNetCore.Mvc;
using RegistroPrestamos.Data;
using RegistroPrestamos.Services;
using Aplicada1.Core;

namespace RegistroPrestamos.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController(LibrosServices librosServices) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<List<Libro>>>> Get()
    {
        var resultado = await librosServices.GetList(l => true);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Result<Libro?>>> Get(int id)
    {
        var resultado = await librosServices.Buscar(id);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<Result>> Post([FromBody] Libro libro)
    {
        var resultado = await librosServices.Guardar(libro);
        return Ok(resultado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Result>> Put(int id, [FromBody] Libro libro)
    {
        if (id != libro.LibroId)
            return BadRequest("El ID de la ruta no coincide con el ID del libro.");

        var resultado = await librosServices.Modificar(libro);
        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Result>> Delete(int id)
    {
        var resultado = await librosServices.Eliminar(id);
        return Ok(resultado);
    }
}
