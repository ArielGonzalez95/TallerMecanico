namespace TallerMecanicoCore.DTO
{
    public class Params
    {
        public string TipoVehiculo { get; set; }
        public int? CantidadPuertas { get; set; }
        public string TipoAuto { get; set; }
        public string Cilindrada { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Patente { get; set; }
        public string[] Desperfecto { get; set; }
        public decimal ManoObra { get; set; }
        public int TiempoReparacion { get; set; }
        public string[] Repuesto { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public decimal? Total { get; set; }
    }
}
