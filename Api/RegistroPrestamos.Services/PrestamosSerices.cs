using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroPrestamos.Data;

namespace RegistroEstudiante.Services;

public class PrestamosServices(IDbContextFactory<RegistrolibrodbContext> contextFactory)
    : IServiceResult<Prestamo, int>
{
    // GUARDAR
    public async Task<bool> Guardar(Prestamo prestamo)
    {
        if (prestamo is null ||
            prestamo.EstudianteId <= 0 ||
            prestamo.LibroId <= 0)
            return false;

        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        var estudianteExiste = await contexto.Estudiantes
            .AnyAsync(e => e.EstudianteId == prestamo.EstudianteId);

        var libroExiste = await contexto.Libros
            .AnyAsync(l => l.LibroId == prestamo.LibroId);

        if (!estudianteExiste || !libroExiste)
            return false;

        var libroPrestado = await contexto.Prestamos
            .AnyAsync(p => p.LibroId == prestamo.LibroId &&
                            p.Estado == "Prestado");

        if (libroPrestado)
            return false;

        prestamo.Estado = "Prestado";

        contexto.Prestamos.Add(prestamo);

        try
        {
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    async Task<Result> IServiceResult<Prestamo, int>.Guardar(
        Prestamo entidad) =>
        await Guardar(entidad)
            ? Result.Success()
            : Result.Failure(
                new Error("SAVE_FAILED", "No se pudo guardar el préstamo."));

    // BUSCAR
    public async Task<Prestamo?> Buscar(int prestamoId)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    async Task<Result<Prestamo?>> IServiceResult<Prestamo, int>.Buscar(
        int id)
    {
        var prestamo = await Buscar(id);

        return prestamo is null
            ? Result.Failure<Prestamo?>(
                new Error("LOAN_NOT_FOUND", "El préstamo no existe."))
            : Result.Success<Prestamo?>(prestamo);
    }

    // EXISTE
    public async Task<bool> Existe(int prestamoId)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .AnyAsync(p => p.PrestamoId == prestamoId);
    }

    // MODIFICAR
    public async Task<bool> Modificar(Prestamo prestamo)
    {
        if (prestamo is null ||
            prestamo.PrestamoId <= 0 ||
            prestamo.EstudianteId <= 0 ||
            prestamo.LibroId <= 0)
            return false;

        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        var existente = await contexto.Prestamos
            .FirstOrDefaultAsync(p =>
                p.PrestamoId == prestamo.PrestamoId);

        if (existente is null)
            return false;

        var libroPrestado = await contexto.Prestamos
            .AnyAsync(p =>
                p.LibroId == prestamo.LibroId &&
                p.Estado == "Prestado" &&
                p.PrestamoId != prestamo.PrestamoId);

        if (libroPrestado)
            return false;

        existente.EstudianteId = prestamo.EstudianteId;
        existente.LibroId = prestamo.LibroId;
        existente.Estado = prestamo.Estado;
        existente.FechaPrestamo = prestamo.FechaPrestamo;
        existente.FechaDevolucion = prestamo.FechaDevolucion;

        try
        {
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> RegistrarDevolucion(int prestamoId)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        var prestamo = await contexto.Prestamos
            .FirstOrDefaultAsync(p =>
                p.PrestamoId == prestamoId);

        if (prestamo is null || prestamo.Estado != "Prestado")
            return false;

        prestamo.Estado = "Devuelto";
        prestamo.FechaDevolucion = DateTime.Now;

        try
        {
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    public async Task<bool> Eliminar(int prestamoId)
    {
        if (prestamoId <= 0)
            return false;

        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        var prestamo = await contexto.Prestamos
            .FirstOrDefaultAsync(p =>
                p.PrestamoId == prestamoId);

        if (prestamo is null)
            return false;

        contexto.Prestamos.Remove(prestamo);

        try
        {
            await contexto.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    async Task<Result> IServiceResult<Prestamo, int>.Eliminar(
        int id) =>
        await Eliminar(id)
            ? Result.Success()
            : Result.Failure(
                new Error("LOAN_NOT_FOUND", "No se pudo eliminar el préstamo."));

    // LISTAR
    public async Task<List<Prestamo>> GetList(
        Expression<Func<Prestamo, bool>> criterio)
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    async Task<Result<List<Prestamo>>> IServiceResult<Prestamo, int>.GetList(
        Expression<Func<Prestamo, bool>> criterio) =>
        Result.Success(await GetList(criterio));

    public async Task<List<Prestamo>> GetPrestamosPendientes()
    {
        await using var contexto =
            await contextFactory.CreateDbContextAsync();

        return await contexto.Prestamos
            .Where(p => p.Estado == "Prestado")
            .AsNoTracking()
            .ToListAsync();
    }

    // OBTENER TODOS
    public async Task<List<Prestamo>> ObtenerTodos()
    {
        return await GetList(p => true);
    }
}