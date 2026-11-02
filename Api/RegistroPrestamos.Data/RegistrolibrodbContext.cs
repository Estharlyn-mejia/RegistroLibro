using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RegistroPrestamos.Data;

public partial class RegistrolibrodbContext : DbContext
{
    public RegistrolibrodbContext(DbContextOptions<RegistrolibrodbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Entrada> Entradas { get; set; }

    public virtual DbSet<EntradasDetalle> EntradasDetalles { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<EstudianteDetalle> EstudianteDetalles { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<PrestamoDetalle> PrestamoDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntradasDetalle>(entity =>
        {
            entity.ToTable("EntradasDetalle");

            entity.HasIndex(e => e.EntradaId, "IX_EntradasDetalle_EntradaId");

            entity.HasIndex(e => e.LibroId, "IX_EntradasDetalle_LibroId");

            entity.HasOne(d => d.Entrada).WithMany(p => p.EntradasDetalles).HasForeignKey(d => d.EntradaId);

            entity.HasOne(d => d.Libro).WithMany(p => p.EntradasDetalles)
                .HasForeignKey(d => d.LibroId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EstudianteDetalle>(entity =>
        {
            entity.HasKey(e => e.DetalleId);

            entity.ToTable("EstudianteDetalle");

            entity.HasIndex(e => e.LibroId, "IX_EstudianteDetalle_LibroId");

            entity.HasIndex(e => e.PrestamoId, "IX_EstudianteDetalle_PrestamoId");

            entity.HasOne(d => d.Libro).WithMany(p => p.EstudianteDetalles).HasForeignKey(d => d.LibroId);

            entity.HasOne(d => d.Prestamo).WithMany(p => p.EstudianteDetalles)
                .HasForeignKey(d => d.PrestamoId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.Property(e => e.Cantidad).HasDefaultValue(1);
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasIndex(e => e.EstudianteId, "IX_Prestamos_EstudianteId");

            entity.HasIndex(e => e.LibroId, "IX_Prestamos_LibroId");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Prestamos).HasForeignKey(d => d.EstudianteId);

            entity.HasOne(d => d.Libro).WithMany(p => p.Prestamos).HasForeignKey(d => d.LibroId);
        });

        modelBuilder.Entity<PrestamoDetalle>(entity =>
        {
            entity.HasIndex(e => e.LibroId, "IX_PrestamoDetalles_LibroId");

            entity.HasIndex(e => e.PrestamoId, "IX_PrestamoDetalles_PrestamoId");

            entity.HasOne(d => d.Libro).WithMany(p => p.PrestamoDetalles)
                .HasForeignKey(d => d.LibroId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Prestamo).WithMany(p => p.PrestamoDetalles).HasForeignKey(d => d.PrestamoId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
