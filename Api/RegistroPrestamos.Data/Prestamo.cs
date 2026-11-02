using System;
using System.Collections.Generic;

namespace RegistroPrestamos.Data;

public partial class Prestamo
{
    public int PrestamoId { get; set; }

    public int EstudianteId { get; set; }

    public int LibroId { get; set; }

    public DateTime FechaPrestamo { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Estudiante Estudiante { get; set; } = null!;

    public virtual ICollection<EstudianteDetalle> EstudianteDetalles { get; set; } = new List<EstudianteDetalle>();

    public virtual Libro Libro { get; set; } = null!;

    public virtual ICollection<PrestamoDetalle> PrestamoDetalles { get; set; } = new List<PrestamoDetalle>();
}
