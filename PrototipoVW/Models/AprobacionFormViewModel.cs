using System.ComponentModel.DataAnnotations;

namespace PrototipoVW.Models;

public class AprobacionFormViewModel
{
    public int IdPropuesta { get; set; }

    public string NombreProyecto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string AreaSolicitante { get; set; } = string.Empty;
    public string? Justificacion { get; set; }
    public string Creador { get; set; } = string.Empty;

    public decimal PresupuestoSolicitado { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El presupuesto aprobado no puede ser negativo.")]
    public decimal PresupuestoAprobado { get; set; }

    public string? ComentarioRevision { get; set; }
}