using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibro.Models;

public class DetallePrestamo
{
    [Key]
    public int DetallePrestamoId { get; set; }
    public int PrestamoId { get; set; }
    public int LibroId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage ="La cantidad debe ser mayor a cero.")]
    public int cantidad { get; set; }
    [ForeignKey("PrestamoId")]
    public Prestamo? Prestamo { get; set; }
    [ForeignKey("LibroId")]
    public Libros? Libro { get; set; }
}