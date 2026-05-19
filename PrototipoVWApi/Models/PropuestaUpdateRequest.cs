namespace PrototipoVWApi.Models
{
    public class PropuestaUpdateRequest
    {
        public string NombreProyecto { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string AreaSolicitante { get; set; } = string.Empty;
        public string? Justificacion { get; set; }
        public decimal PresupuestoSolicitado { get; set; }
    }
}
