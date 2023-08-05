using System;
using System.Collections.Generic;

namespace TallerMecanicoCore.Models;

public partial class Presupuesto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Email { get; set; }

    public decimal? Total { get; set; }

    public int? IdVehiculo { get; set; }

    public virtual ICollection<Desperfecto> Desperfectos { get; set; } = new List<Desperfecto>();

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
