using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibro.Models;

public class EstudianteDetalle
{
    [Key]
    public int DetalleId { get; set; }
    public int PrestamoId { get; set; }
    public int LibroId { get; set; }
    [Required(ErrorMessage = "El numero es obligaatorio")]
    public string Telefono { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "La canitdad debe ser mayor que cero.")]
    public int Cantidad { get; set; }
    [ForeignKey(nameof(PrestamoId))]
    public Prestamo? Prestamo { get; set; }
    [ForeignKey(nameof(LibroId))]
    public Libros? Libro { get; set; }
}