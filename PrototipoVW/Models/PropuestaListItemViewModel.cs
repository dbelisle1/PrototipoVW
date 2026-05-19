namespace PrototipoVW.Models;

public class PropuestaListItemViewModel
{
    public int IdPropuesta { get; set; }

    public int IdCreador { get; set; }
    public string Creador { get; set; } = string.Empty;

    public string NombreProyecto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string AreaSolicitante { get; set; } = string.Empty;

    public decimal PresupuestoSolicitado { get; set; }
    public decimal PresupuestoAprobado { get; set; }

    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}
