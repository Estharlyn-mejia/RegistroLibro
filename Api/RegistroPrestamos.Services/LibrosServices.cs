using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroPrestamos.Data;

namespace RegistroPrestamos.Services;

public class LibrosServices(IDbContextFactory<RegistrolibrodbContext> contextFactory
    ) : IServiceResult<Libro, int>
{
    public async Task<Result<Libro?>> Buscar(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .FirstOrDefaultAsync(l => l.LibroId == libroId);
    }

    public async Task<Result<List<Libro>>> GetList(Expression<Func<Libro, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> Existe(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AnyAsync(l => l.LibroId == libroId);
    }

    public async Task<bool> Insertar(Libro libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Libros.Add(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Libro libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result> Eliminar (int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var eliminado = await contexto.Libros
            .Where(l => l.LibroId == libroId)
            .ExecuteDeleteAsync() > 0;
        return Result.Success(eliminado);
    }

    public async Task<Result> Guardar(Libro libro)
    {
        if (!await Existe(libro.LibroId))
        {
            return Result.Success(await Insertar(libro));
        }
        else
        {
            return Result.Success(await Modificar(libro));
        }
    }
}