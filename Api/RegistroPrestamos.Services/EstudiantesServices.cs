using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroPrestamos.Data;

namespace RegistroEstudiante.Services;

public class EstudiantesServices(IDbContextFactory<RegistrolibrodbContext> contextFactory
    ) : IServiceResult<Estudiante, int>
{
    public async Task<Result<Estudiante?>> Buscar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .FirstOrDefaultAsync(e => e.EstudianteId == estudianteId);
    }

    public async Task<Result<List<Estudiante>>> GetList(Expression<Func<Estudiante, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Existe(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .AnyAsync(e => e.EstudianteId == estudianteId);
    }

    public async Task<bool> Insertar(Estudiante estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Estudiantes.Add(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Estudiante estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Estudiantes
            .Where(e => e.EstudianteId == estudianteId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task<Result> Guardar(Estudiante estudiante)
    {
        if(!await Existe(estudiante.EstudianteId))
        {
            return Result.Success(await Insertar(estudiante));
        }
        else
        {
            return Result.Success(await Modificar(estudiante));
        }
    }
}