using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibro.Context;
using RegistroLibro.Models;

public class PrestamosServices(IDbContextFactory<Contexto> contextFactory)
    : IServiceResult<Prestamo, int>
{
    public Task<bool> Guardar(Prestamo prestamo) => Guardar(prestamo,
        prestamo.LibroId > 0 ? [new PrestamoDetalle { LibroId = prestamo.LibroId, Cantidad = 1 }] : []);

    public async Task<bool> Guardar(Prestamo prestamo, List<PrestamoDetalle> detalles)
    {
        var solicitados = NormalizarDetalles(detalles);
        if (prestamo.EstudianteId <= 0 || solicitados.Count == 0 || !EstadoValido(prestamo.Estado)) return false;

        await using var contexto = await contextFactory.CreateDbContextAsync();
        await using var transaccion = await contexto.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var libros = await contexto.Libros.Where(l => solicitados.Select(d => d.LibroId).Contains(l.LibroId)).ToDictionaryAsync(l => l.LibroId);
        if (libros.Count != solicitados.Count || solicitados.Any(d => libros[d.LibroId].Cantidad < d.Cantidad)) return false;

        if (prestamo.Estado == "Prestado")
            foreach (var detalle in solicitados) libros[detalle.LibroId].Cantidad -= detalle.Cantidad;
        prestamo.LibroId = solicitados[0].LibroId;
        prestamo.Detalles = solicitados.Select(d => new PrestamoDetalle { LibroId = d.LibroId, Cantidad = d.Cantidad }).ToList();
        contexto.Prestamos.Add(prestamo);
        try
        {
            await contexto.SaveChangesAsync();
            await transaccion.CommitAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public Task<bool> GuardarDetalles(Prestamo prestamo) => Guardar(prestamo, prestamo.Detalles);

    async Task<Result> IServiceResult<Prestamo, int>.Guardar(Prestamo entidad) =>
        await Guardar(entidad) ? Result.Success() : Result.Failure(new Error("SAVE_FAILED", "No se pudo guardar el préstamo; revise las cantidades disponibles."));

    public async Task<bool> EstaPrestado(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.PrestamoDetalles.AnyAsync(d => d.LibroId == libroId && d.Prestamo != null && d.Prestamo.Estado == "Prestado");
    }

    public async Task<Result> RegistrarDevolucion(int prestamoId, int estudianteId, DateTime fechaDevolucion)
    {
        if (prestamoId <= 0 || estudianteId <= 0 || fechaDevolucion == default || fechaDevolucion.Date > DateTime.Today)
            return Result.Failure(new Error("VALIDATION", "El préstamo, el estudiante y una fecha válida son obligatorios."));

        await using var contexto = await contextFactory.CreateDbContextAsync();
        await using var transaccion = await contexto.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var prestamo = await contexto.Prestamos.Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId && p.EstudianteId == estudianteId && p.Estado == "Prestado");
        if (prestamo is null) return Result.Failure(new Error("LOAN_NOT_FOUND", "No existe un préstamo activo para ese estudiante."));
        if (fechaDevolucion.Date < prestamo.FechaPrestamo.Date)
            return Result.Failure(new Error("INVALID_RETURN_DATE", "La fecha de devolución no puede ser anterior a la del préstamo."));

        var detalles = DetallesDelPrestamo(prestamo);
        var libros = await contexto.Libros.Where(l => detalles.Select(d => d.LibroId).Contains(l.LibroId)).ToDictionaryAsync(l => l.LibroId);
        if (libros.Count != detalles.Select(d => d.LibroId).Distinct().Count())
            return Result.Failure(new Error("BOOK_NOT_FOUND", "No se encontraron todos los libros del préstamo."));
        foreach (var detalle in detalles) libros[detalle.LibroId].Cantidad += detalle.Cantidad;
        prestamo.Estado = "Devuelto";
        prestamo.FechaDevolucion = fechaDevolucion;
        try
        {
            await contexto.SaveChangesAsync();
            await transaccion.CommitAsync();
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
        return await contexto.Prestamos.Include(p => p.Detalles).AsNoTracking()
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    async Task<Result<Prestamo?>> IServiceResult<Prestamo, int>.Buscar(int id)
    {
        var prestamo = await Buscar(id);
        return prestamo is null ? Result.Failure<Prestamo?>(new Error("LOAN_NOT_FOUND", "El préstamo no existe.")) : Result.Success<Prestamo?>(prestamo);
    }

    public Task<bool> Modificar(Prestamo prestamo) => Modificar(prestamo,
        prestamo.Detalles.Count > 0 ? prestamo.Detalles :
        prestamo.LibroId > 0 ? [new PrestamoDetalle { LibroId = prestamo.LibroId, Cantidad = 1 }] : []);

    public async Task<bool> Modificar(Prestamo prestamo, List<PrestamoDetalle> nuevosDetalles)
    {
        var solicitados = NormalizarDetalles(nuevosDetalles);
        if (prestamo.PrestamoId <= 0 || prestamo.EstudianteId <= 0 || solicitados.Count == 0 || !EstadoValido(prestamo.Estado)) return false;

        await using var contexto = await contextFactory.CreateDbContextAsync();
        await using var transaccion = await contexto.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var existente = await contexto.Prestamos.Include(p => p.Detalles).FirstOrDefaultAsync(p => p.PrestamoId == prestamo.PrestamoId);
        if (existente is null) return false;

        var anteriores = DetallesDelPrestamo(existente);
        var ids = anteriores.Concat(solicitados).Select(d => d.LibroId).Distinct().ToList();
        var libros = await contexto.Libros.Where(l => ids.Contains(l.LibroId)).ToDictionaryAsync(l => l.LibroId);
        if (libros.Count != ids.Count) return false;

        if (existente.Estado == "Prestado")
            foreach (var detalle in anteriores) libros[detalle.LibroId].Cantidad += detalle.Cantidad;

        if (prestamo.Estado == "Prestado")
        {
            foreach (var detalle in solicitados)
                if (libros[detalle.LibroId].Cantidad < detalle.Cantidad) return false;
            foreach (var detalle in solicitados) libros[detalle.LibroId].Cantidad -= detalle.Cantidad;
        }

        existente.EstudianteId = prestamo.EstudianteId;
        existente.LibroId = solicitados[0].LibroId;
        existente.FechaPrestamo = prestamo.FechaPrestamo;
        existente.FechaDevolucion = prestamo.FechaDevolucion;
        existente.Estado = prestamo.Estado;
        contexto.PrestamoDetalles.RemoveRange(existente.Detalles);
        existente.Detalles = solicitados.Select(d => new PrestamoDetalle { LibroId = d.LibroId, Cantidad = d.Cantidad }).ToList();
        try
        {
            await contexto.SaveChangesAsync();
            await transaccion.CommitAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> Eliminar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        await using var transaccion = await contexto.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var prestamo = await contexto.Prestamos.Include(p => p.Detalles).FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
        if (prestamo is null) return false;
        if (prestamo.Estado == "Prestado")
        {
            var detalles = DetallesDelPrestamo(prestamo);
            var libros = await contexto.Libros.Where(l => detalles.Select(d => d.LibroId).Contains(l.LibroId)).ToDictionaryAsync(l => l.LibroId);
            if (libros.Count != detalles.Select(d => d.LibroId).Distinct().Count()) return false;
            foreach (var detalle in detalles) libros[detalle.LibroId].Cantidad += detalle.Cantidad;
        }
        contexto.Prestamos.Remove(prestamo);
        try
        {
            await contexto.SaveChangesAsync();
            await transaccion.CommitAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    async Task<Result> IServiceResult<Prestamo, int>.Eliminar(int id) =>
        await Eliminar(id) ? Result.Success() : Result.Failure(new Error("LOAN_NOT_FOUND", "El préstamo no existe."));

    public async Task<List<Prestamo>> GetList(Expression<Func<Prestamo, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.Include(p => p.Detalles).Where(criterio).AsNoTracking().ToListAsync();
    }

    async Task<Result<List<Prestamo>>> IServiceResult<Prestamo, int>.GetList(Expression<Func<Prestamo, bool>> criterio) => Result.Success(await GetList(criterio));

    public async Task<List<Prestamo>> GetPrestamosPendientes(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.Include(p => p.Detalles).Where(p => p.EstudianteId == estudianteId && p.Estado == "Prestado").OrderBy(p => p.PrestamoId).AsNoTracking().ToListAsync();
    }

    public async Task<List<Prestamo>> ObtenerTodos() => await GetList(p => p.PrestamoId > 0);

    private static bool EstadoValido(string estado) => estado is "Prestado" or "Devuelto";

    private static List<PrestamoDetalle> NormalizarDetalles(IEnumerable<PrestamoDetalle> detalles)
    {
        var lineas = detalles.ToList();
        if (lineas.Count == 0 || lineas.Any(d => d.LibroId <= 0 || d.Cantidad <= 0)) return [];
        return lineas.GroupBy(d => d.LibroId)
            .Select(g => new PrestamoDetalle { LibroId = g.Key, Cantidad = g.Sum(d => d.Cantidad) })
            .ToList();
    }

    private static List<PrestamoDetalle> DetallesDelPrestamo(Prestamo prestamo) => prestamo.Detalles.Count > 0
        ? NormalizarDetalles(prestamo.Detalles)
        : prestamo.LibroId > 0 ? [new PrestamoDetalle { LibroId = prestamo.LibroId, Cantidad = 1 }] : [];
}
