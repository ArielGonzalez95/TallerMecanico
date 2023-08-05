using System;
using System.Collections.Generic;

namespace TallerMecanicoCore.Models;

public partial class Moto
{
    public int Id { get; set; }

    public int? IdVehiculo { get; set; }

    public int? Cilindrada { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
