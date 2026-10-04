using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RegistroLibro.Context;
using RegistroLibro.Models;
public class PrestamosServices(IDbContextFactory<Contexto> contextFactory){
    
    public async Task<bool> Guardar(Prestamo prestamo)
    {
        await using var Contexto = await contextFactory.CreateDbContextAsync();
        Contexto.Prestamos.Add(prestamo);
        return await Contexto.SaveChangesAsync() > 0;
    }

    public async Task<Prestamo?> Buscar(int prestamoId)
    {
        await using var Contexto = await contextFactory.CreateDbContextAsync();
        return await Contexto.Prestamos
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    private async Task<bool> Existe(int prestamoId)
    {
        await using var Contexto = await contextFactory.CreateDbContextAsync();
        return await Contexto.Prestamos.AnyAsync(l => l.PrestamoId == prestamoId);
    }

    public async Task<bool> Modificar(Prestamo prestamo)
    {
        await using var Contexto = await contextFactory.CreateDbContextAsync();
        Contexto.Prestamos.Update(prestamo);
        return await Contexto.SaveChangesAsync() >0 ;
    }

    public async Task<bool> Eliminar(int prestamoId)
    {
        await using var Contexto = await contextFactory.CreateDbContextAsync();
        return await Contexto.Prestamos.AsNoTracking().Where(l => l.PrestamoId == prestamoId).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Prestamo>> GetList(Expression<Func<Prestamo, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
    
        return await contexto.Prestamos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
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
        await using var Contexto = await contextFactory.CreateDbContextAsync();
        return await Contexto.Prestamos.AsNoTracking().ToListAsync();
    }
}