using System.ComponentModel.DataAnnotations;

namespace RegistroLibro.Models;

public partial class Prestamo
{
    [Key]
    public int PrestamoId { get; set; }

    [Required(ErrorMessage ="Este campo esta vacio")]
    public int EstudianteId { get; set; }

    [Required(ErrorMessage ="Este campo esta vacio")]
    public int LibroId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateTime FechaPrestamo { get; set;}

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateTime? FechaDevolucion { get; set;}

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public String Estado { get; set;} = string.Empty;
}