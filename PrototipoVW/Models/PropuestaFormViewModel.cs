using System.ComponentModel.DataAnnotations;

namespace PrototipoVW.Models;

public class PropuestaFormViewModel
{
    public int IdPropuesta { get; set; }

    [Required(ErrorMessage = "Ingrese el nombre del proyecto.")]
    public string NombreProyecto { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Ingrese el área solicitante.")]
    public string AreaSolicitante { get; set; } = string.Empty;

    public string? Justificacion { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El presupuesto solicitado no puede ser negativo.")]
    public decimal PresupuestoSolicitado { get; set; }
}