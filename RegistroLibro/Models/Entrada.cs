using System.ComponentModel.DataAnnotations;

namespace RegistroLibro.Models;

public class Entrada
{
    [Key]
    public int EntradaId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "El concepto es obligatorio")]
    public string Concepto { get; set; } = string.Empty;

    // Total representa el total de unidades recibidas, porque el detalle no registra precios.
    public int Total { get; set; }

    public List<EntradaDetalle> Detalles { get; set; } = [];
}
