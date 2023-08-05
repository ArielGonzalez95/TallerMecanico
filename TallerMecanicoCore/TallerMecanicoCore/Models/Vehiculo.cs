using System;
using System.Collections.Generic;

namespace TallerMecanicoCore.Models;

public partial class Vehiculo
{
    public int Id { get; set; }

    public string? Marca { get; set; }

    public string? Modelo { get; set; }

    public string? Patente { get; set; }

    public virtual ICollection<Automovil> Automovils { get; set; } = new List<Automovil>();

    public virtual ICollection<Moto> Motos { get; set; } = new List<Moto>();

    public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
}
