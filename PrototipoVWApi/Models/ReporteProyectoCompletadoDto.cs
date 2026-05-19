namespace PrototipoVWApi.Models
{
    public class ReporteProyectoCompletadoDto
    {
        public int IdPropuesta { get; set; }
        public string NombreProyecto { get; set; } = string.Empty;
        public string AreaSolicitante { get; set; } = string.Empty;
        public decimal PresupuestoAprobado { get; set; }
        public DateTime? FechaCompletado { get; set; }
        public string? CompletadoPor { get; set; }
    }
}
