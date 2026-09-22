using Microsoft.EntityFrameworkCore;
using RegistroLibro.Models;

namespace RegistroLibro.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public DbSet<Libros> Libros { get; set; }
}