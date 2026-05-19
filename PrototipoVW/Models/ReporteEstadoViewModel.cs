namespace PrototipoVW.Models
{
    public class ReporteEstadoViewModel
    {
        public string Estado { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PresupuestoSolicitado { get; set; }
        public decimal PresupuestoAprobado { get; set; }
    }
}
