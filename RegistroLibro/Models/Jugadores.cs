using System.ComponentModel.DataAnnotations;


namespace Jugador.Model;

public class Jugadores
{
    [Key]

    public int JugadorId { get; set; }
    [Required(ErrorMessage ="Este campo es invalido")]
    public string Jugador { get; set; } = string.Empty;
    [Required(ErrorMessage ="Este campo es invalido")]
    public int PuntosJugador { get; set; }
}