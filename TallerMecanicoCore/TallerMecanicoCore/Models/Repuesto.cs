using System;
using System.Collections.Generic;

namespace TallerMecanicoCore.Models;

public partial class Repuesto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public decimal? Precio { get; set; }

    public virtual ICollection<Desperfecto> IdDesperfectos { get; set; } = new List<Desperfecto>();
}
