namespace PrototipoVWApi.Models
{
    public class PropuestaDto
    {
        public int IdPropuesta { get; set; }

        public int IdCreador { get; set; }
        public string Creador { get; set; } = string.Empty;

        public int? IdRevisor { get; set; }
        public string? Revisor { get; set; }

        public int? IdCompletadoPor { get; set; }
        public string? CompletadoPor { get; set; }

        public string NombreProyecto { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string AreaSolicitante { get; set; } = string.Empty;
        public string? Justificacion { get; set; }

        public decimal PresupuestoSolicitado { get; set; }
        public decimal PresupuestoAprobado { get; set; }

        public string Estado { get; set; } = string.Empty;
        public string? ComentarioRevision { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaRevision { get; set; }
        public DateTime? FechaCompletado { get; set; }
    }
}
