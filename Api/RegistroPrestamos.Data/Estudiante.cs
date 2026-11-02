using System;
using System.Collections.Generic;

namespace RegistroPrestamos.Data;

public partial class Estudiante
{
    public int EstudianteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Emil { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
