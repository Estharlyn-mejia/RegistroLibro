using Microsoft.EntityFrameworkCore;
using RegistroLibro.Models;

namespace RegistroLibro.Context;
public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }
    public DbSet<Libros> Libros { get; set; }
    public DbSet<Estudiantes> Estudiantes{get; set;}
    public DbSet<Prestamo> Prestamos{get; set;}

    public DbSet<Devolver> Devolvers{get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prestamo>()
            .HasIndex(prestamo => prestamo.LibroId)
            .IsUnique()
            .HasFilter("[Estado] = N'Prestado'");
    }
}