namespace PrototipoVWApi.Models
{
    public class ReporteDashboardDto
    {
        public ReporteResumenDto Resumen { get; set; } = new();

        public List<ReporteEstadoDto> PropuestasPorEstado { get; set; } = new();

        public List<ReporteAreaDto> PresupuestoPorArea { get; set; } = new();

        public List<ReporteProyectoCompletadoDto> ProyectosCompletados { get; set; } = new();
    }
}
