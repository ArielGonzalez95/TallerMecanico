using System;
using System.Collections.Generic;

namespace TallerMecanicoCore.Models;

public partial class Automovil
{
    public int Id { get; set; }

    public int? IdVehiculo { get; set; }

    public string? Tipo { get; set; }

    public int? CantidadPuertas { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
