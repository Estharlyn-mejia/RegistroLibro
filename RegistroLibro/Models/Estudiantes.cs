using System.ComponentModel.DataAnnotations;

namespace RegistroLibro.Models;


public class Estudiantes
{
    [Key]
    public int EstudianteId { get; set; }
    [Required(ErrorMessage ="ID invalido")]
    public string? Nombre { get; set; } = string.Empty;
    [Required(ErrorMessage ="El nombre es invalido")]
    public string? Direccion { get; set; } = string.Empty;
    [Required(ErrorMessage ="Direccion invalida")]
    public string? Emil { get; set; } = string.Empty;
    [Required(ErrorMessage ="Email esta incorrecto")]

    public DateTime Fecha{get; set;}
}