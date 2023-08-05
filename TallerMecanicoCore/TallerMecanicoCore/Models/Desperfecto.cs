using System;
using System.Collections.Generic;

namespace TallerMecanicoCore.Models;

public partial class Desperfecto
{
    public int Id { get; set; }

    public int? IdPresupuesto { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Manodeobra { get; set; }

    public int? Tiempo { get; set; }

    public virtual Presupuesto? IdPresupuestoNavigation { get; set; }

    public virtual ICollection<Repuesto> IdRepuestos { get; set; } = new List<Repuesto>();
}
