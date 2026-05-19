namespace PrototipoVWApi.Models
{
    public class ReporteAreaDto
    {
        public string AreaSolicitante { get; set; } = string.Empty;
        public int CantidadPropuestas { get; set; }
        public decimal PresupuestoSolicitado { get; set; }
        public decimal PresupuestoAprobado { get; set; }
    }
}
