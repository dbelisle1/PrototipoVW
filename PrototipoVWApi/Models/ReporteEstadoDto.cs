namespace PrototipoVWApi.Models
{
    public class ReporteEstadoDto
    {
        public string Estado { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PresupuestoSolicitado { get; set; }
        public decimal PresupuestoAprobado { get; set; }
    }
}
