using System;
using System.Collections.Generic;

namespace RegistroPrestamos.Data;

public partial class PrestamoDetalle
{
    public int PrestamoDetalleId { get; set; }

    public int PrestamoId { get; set; }

    public int LibroId { get; set; }

    public int Cantidad { get; set; }

    public virtual Libro Libro { get; set; } = null!;

    public virtual Prestamo Prestamo { get; set; } = null!;
}
