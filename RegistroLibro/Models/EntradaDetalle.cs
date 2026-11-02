using System.ComponentModel.DataAnnotations;

namespace RegistroLibro.Models;

public class EntradaDetalle
{
    [Key]
    public int Id { get; set; }
    public int EntradaId { get; set; }
    public int LibroId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
    public int Cantidad { get; set; }

    public Entrada? Entrada { get; set; }
    public Libros? Libro { get; set; }
}
