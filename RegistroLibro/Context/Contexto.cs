using Microsoft.EntityFrameworkCore;
using RegistroLibro.Models;

namespace RegistroLibro.Context;

public class Contexto(DbContextOptions<Contexto> options) : DbContext(options)
{
    public DbSet<Libros> Libros { get; set; }
    public DbSet<Estudiantes> Estudiantes { get; set; }
    public DbSet<Prestamo> Prestamos { get; set; }
    public DbSet<PrestamoDetalle> PrestamoDetalles { get; set; }
    public DbSet<Entrada> Entradas { get; set; }
    public DbSet<EntradaDetalle> EntradasDetalle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<EstudianteDetalle>();
        modelBuilder.Entity<Prestamo>()
            .HasIndex(prestamo => prestamo.LibroId);

        modelBuilder.Entity<EstudianteDetalle>()
            .HasOne(detalle => detalle.Prestamo)
            .WithMany()
            .HasForeignKey(detalle => detalle.PrestamoId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PrestamoDetalle>()
            .HasOne(detalle => detalle.Prestamo)
            .WithMany(prestamo => prestamo.Detalles)
            .HasForeignKey(detalle => detalle.PrestamoId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PrestamoDetalle>()
            .HasOne(detalle => detalle.Libro)
            .WithMany()
            .HasForeignKey(detalle => detalle.LibroId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EntradaDetalle>()
            .HasOne(detalle => detalle.Entrada)
            .WithMany(entrada => entrada.Detalles)
            .HasForeignKey(detalle => detalle.EntradaId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<EntradaDetalle>()
            .HasOne(detalle => detalle.Libro)
            .WithMany()
            .HasForeignKey(detalle => detalle.LibroId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
