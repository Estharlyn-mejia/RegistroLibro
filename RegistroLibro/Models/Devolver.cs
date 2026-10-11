using System.ComponentModel.DataAnnotations;

namespace RegistroLibro.Models;

public class Devolver
{
	[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un préstamo válido")]
	public int PrestamoId { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante válido")]
	public int EstudianteId { get; set; }

	[Required(ErrorMessage = "La fecha de devolución es obligatoria")]
	public DateTime FechaDevolucion { get; set; } = DateTime.Today;
}
