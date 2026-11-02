using System;
using System.Collections.Generic;

namespace RegistroPrestamos.Data;

public partial class Entrada
{
    public int EntradaId { get; set; }

    public DateTime Fecha { get; set; }

    public string Concepto { get; set; } = null!;

    public int Total { get; set; }

    public virtual ICollection<EntradasDetalle> EntradasDetalles { get; set; } = new List<EntradasDetalle>();
}
