using RegistroLibro.Models;
using RegistroLibro.Context;
using Microsoft.EntityFrameworkCore;


public class EstudianteServices(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Existe(int estudianteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Estudiantes.AnyAsync(l => l.EstudianteId == estudianteId);
    }

    public async Task<bool> Guardar(Estudiantes estudiante)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Estudiantes.Add(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Estudiantes estudiante)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Estudiantes.Update(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Estudiantes?> Buscar(int estudianteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Estudiantes.Include(l => l.EstudianteId).Include(l => l.Nombre).FirstOrDefaultAsync(l => l.EstudianteId == estudianteId);
    }

    public async Task<bool> Eliminar(int estudianteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Estudiantes.AsNoTracking().Where(p => p.EstudianteId == estudianteId).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Estudiantes>> ObtenerTodo()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Estudiantes.AsNoTracking().ToListAsync();
    }
}