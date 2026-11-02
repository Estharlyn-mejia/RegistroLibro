
using RegistroLibro.Models;
using RegistroLibro.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class EntradasServices(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Existe(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .AnyAsync(e => e.EntradaId == entradaId);
    }

    public async Task<bool> Guardar(Entrada entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Entradas.Add(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Entrada entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Entradas.Update(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Entrada?> Buscar(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.Detalles)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<bool> Eliminar(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .AsNoTracking()
            .Where(e => e.EntradaId == entradaId)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Entrada>> GetList(
        Expression<Func<Entrada, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        return await contexto.Entradas
            .Include(e => e.Detalles)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Entrada>> ObtenerTodos()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.Detalles)
            .AsNoTracking()
            .ToListAsync();
    }
}
