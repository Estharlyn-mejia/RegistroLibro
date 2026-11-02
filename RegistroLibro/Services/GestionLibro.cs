using RegistroLibro.Models;
using RegistroLibro.Context;
using Microsoft.EntityFrameworkCore;

public class GestionLibro(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Existe(int libroId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros.AnyAsync(l => l.LibroId == libroId);
    }

    public async Task<bool> Guardar(Libros libro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Libros.Add(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Libros libro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Libros?> Buscar(int libroId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros
            .FirstOrDefaultAsync(p => p.LibroId == libroId);
    }

    public async Task<bool> Eliminar(int libroId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        bool tienePrestamos = await contexto.Prestamos.AnyAsync(p => p.LibroId == libroId) ||
            await contexto.PrestamoDetalles.AnyAsync(d => d.LibroId == libroId);
        bool tieneEntradas = await contexto.EntradasDetalle.AnyAsync(d => d.LibroId == libroId);
        if (tienePrestamos || tieneEntradas)
        {
            return false;
        }

        return await contexto.Libros.AsNoTracking()
            .Where(p => p.LibroId == libroId).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Libros>> ObtenerTodos()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AsNoTracking().ToListAsync();
    }
}
