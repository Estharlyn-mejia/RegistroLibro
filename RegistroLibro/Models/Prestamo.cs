using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibro.Models;

public partial class Prestamo
{
    [Key]
    public int PrestamoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante válido")]
    public int EstudianteId { get; set; }

    [ForeignKey(nameof(EstudianteId))]
    public virtual Estudiantes? Estudiante { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un libro válido")]
    public int LibroId { get; set; }

    [ForeignKey(nameof(LibroId))]
    public virtual Libros? Libro { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateTime FechaPrestamo { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Estado { get; set; } = string.Empty;
}