using System.ComponentModel.DataAnnotations;

namespace RegistroLibro.Models;

public class Libros
{
    [Key]
    public int LibroId { get; set; }
    [Required(ErrorMessage ="Este campo esta vacio")]
    public string Titulo { get; set; } = string.Empty;
    [Required(ErrorMessage ="El campo autor esta vacio")]
    public string Autor { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro válido")]
    public int AnoPublicacion { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro válido")]
    public int Cantidad { get; set; }
}