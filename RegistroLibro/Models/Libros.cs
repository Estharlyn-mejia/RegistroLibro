using System.ComponentModel.DataAnnotations;

namespace RegistroLibro.Models;

public class Libros
{
    [Key]
    public int LibroId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public int AnoPublicacion { get; set; }
}