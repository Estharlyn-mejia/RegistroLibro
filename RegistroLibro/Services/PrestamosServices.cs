using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibro.Context;
using RegistroLibro.Models;

public class PrestamosServices(IDbContextFactory<Contexto> contextFactory)
    : IServiceResult<Prestamo, int>
{
    public async Task<bool> Guardar(Prestamo prestamo)
    {
        if (prestamo.EstudianteId <= 0 || prestamo.LibroId <= 0)
        {
            return false;
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();
        if (await contexto.Prestamos.AnyAsync(p =>
                p.LibroId == prestamo.LibroId && p.Estado == "Prestado"))
        {
            return false;
        }

        contexto.Prestamos.Add(prestamo);
        try
        {
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    async Task<Result> IServiceResult<Prestamo, int>.Guardar(Prestamo entidad)
    {
        return await Guardar(entidad)
            ? Result.Success()
            : Result.Failure(new Error("SAVE_FAILED", "No se pudo guardar el préstamo; verifique que el libro no esté prestado."));
    }

    public async Task<bool> EstaPrestado(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.AnyAsync(p =>
            p.LibroId == libroId && p.Estado == "Prestado");
    }

    public async Task<Result> RegistrarDevolucion(int prestamoId, int estudianteId, DateTime fechaDevolucion)
    {
        if (prestamoId <= 0 || estudianteId <= 0)
        {
            return Result.Failure(new Error("VALIDATION", "El préstamo y el estudiante son obligatorios."));
        }
        if (fechaDevolucion == default || fechaDevolucion.Date > DateTime.Today)
        {
            return Result.Failure(new Error("INVALID_RETURN_DATE", "La fecha de devolución no es válida."));
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();
        var prestamo = await contexto.Prestamos.FirstOrDefaultAsync(p =>
            p.PrestamoId == prestamoId &&
            p.EstudianteId == estudianteId &&
            p.Estado == "Prestado");

        if (prestamo == null)
        {
            return Result.Failure(new Error("LOAN_NOT_FOUND", "No existe un préstamo activo para ese estudiante."));
        }
        if (fechaDevolucion.Date < prestamo.FechaPrestamo.Date)
        {
            return Result.Failure(new Error("INVALID_RETURN_DATE", "La fecha de devolución no puede ser anterior a la fecha del préstamo."));
        }

        prestamo.Estado = "Devuelto";
        prestamo.FechaDevolucion = fechaDevolucion;
        try
        {
            await contexto.SaveChangesAsync();
            return Result.Success();
        }
        catch (DbUpdateException)
        {
            return Result.Failure(new Error("RETURN_FAILED", "No se pudo registrar la devolución."));
        }
    }

    public async Task<Prestamo?> Buscar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    async Task<Result<Prestamo?>> IServiceResult<Prestamo, int>.Buscar(int id)
    {
        var prestamo = await Buscar(id);
        return prestamo is null
            ? Result.Failure<Prestamo?>(new Error("LOAN_NOT_FOUND", "El préstamo no existe."))
            : Result.Success<Prestamo?>(prestamo);
    }

    public async Task<bool> Modificar(Prestamo prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        if (prestamo.Estado == "Prestado" && await contexto.Prestamos.AnyAsync(p =>
                p.LibroId == prestamo.LibroId &&
                p.Estado == "Prestado" &&
                p.PrestamoId != prestamo.PrestamoId))
        {
            return false;
        }

        contexto.Prestamos.Update(prestamo);
        try
        {
            return await contexto.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> Eliminar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.AsNoTracking()
            .Where(p => p.PrestamoId == prestamoId)
            .ExecuteDeleteAsync() > 0;
    }

    async Task<Result> IServiceResult<Prestamo, int>.Eliminar(int id)
    {
        return await Eliminar(id)
            ? Result.Success()
            : Result.Failure(new Error("LOAN_NOT_FOUND", "El préstamo no existe."));
    }

    public async Task<List<Prestamo>> GetList(Expression<Func<Prestamo, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    async Task<Result<List<Prestamo>>> IServiceResult<Prestamo, int>.GetList(
        Expression<Func<Prestamo, bool>> criterio)
    {
        return Result.Success(await GetList(criterio));
    }

    public async Task<List<Prestamo>> GetPrestamosPendientes(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .Where(p => p.EstudianteId == estudianteId && p.Estado == "Prestado")
            .OrderBy(p => p.PrestamoId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Prestamo>> ObtenerTodos()
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.AsNoTracking().ToListAsync();
    }
}