using System;
using System.Collections.Generic;

namespace RegistroPrestamos.Data;

public partial class EstudianteDetalle
{
    public int DetalleId { get; set; }

    public int PrestamoId { get; set; }

    public int LibroId { get; set; }

    public string Telefono { get; set; } = null!;

    public int Cantidad { get; set; }

    public virtual Libro Libro { get; set; } = null!;

    public virtual Prestamo Prestamo { get; set; } = null!;
}
