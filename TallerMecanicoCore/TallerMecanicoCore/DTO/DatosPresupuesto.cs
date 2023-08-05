namespace TallerMecanicoCore.DTO
{
    public class DatosPresupuesto
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public decimal? Total { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Patente { get; set; }
        public int? CantidadPuertas { get; set; }
        public int? Cilindrada { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Manodeobra { get; set; }
        public int? Tiempo { get; set; }
        public string? NombreRepuesto { get; set; }
        public decimal? Precio { get; set; }
    }
}

