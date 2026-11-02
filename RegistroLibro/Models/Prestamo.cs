using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibro.Models;

public class Prestamo
{
    [Key]
    public int PrestamoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante válido")]
    public int EstudianteId { get; set; }

    [ForeignKey(nameof(EstudianteId))]
    public Estudiantes? Estudiante { get; set; }

    // Se conserva por compatibilidad con préstamos existentes; corresponde al primer detalle.
    public int LibroId { get; set; }

    [ForeignKey(nameof(LibroId))]
    public Libros? Libro { get; set; }

    [Required]
    public DateTime FechaPrestamo { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    [Required]
    public string Estado { get; set; } = "Prestado";

    public List<PrestamoDetalle> Detalles { get; set; } = [];
}
