namespace PrototipoVWApi.Models
{
    public class ReporteResumenDto
    {
        public int TotalPropuestas { get; set; }
        public int TotalPendientes { get; set; }
        public int TotalAprobadas { get; set; }
        public int TotalRechazadas { get; set; }
        public int TotalCompletadas { get; set; }

        public decimal PresupuestoTotalSolicitado { get; set; }
        public decimal PresupuestoTotalAprobado { get; set; }
    }
}
