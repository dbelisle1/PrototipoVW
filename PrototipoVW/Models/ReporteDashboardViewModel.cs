namespace PrototipoVW.Models
{
    public class ReporteDashboardViewModel
    {
        public ReporteResumenViewModel Resumen { get; set; } = new();

        public List<ReporteEstadoViewModel> PropuestasPorEstado { get; set; } = new();

        public List<ReporteAreaViewModel> PresupuestoPorArea { get; set; } = new();

        public List<ReporteProyectoCompletadoViewModel> ProyectosCompletados { get; set; } = new();
    }
}
