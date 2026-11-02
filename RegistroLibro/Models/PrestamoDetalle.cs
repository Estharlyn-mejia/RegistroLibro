using System.ComponentModel.DataAnnotations;

namespace RegistroLibro.Models;

public class PrestamoDetalle
{
    [Key]
    public int PrestamoDetalleId { get; set; }
    public int PrestamoId { get; set; }
    public int LibroId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
    public int Cantidad { get; set; }

    public Prestamo? Prestamo { get; set; }
    public Libros? Libro { get; set; }
}
