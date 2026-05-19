using System.ComponentModel.DataAnnotations;

namespace PrototipoVW.Models
{
    public class UsuarioFormViewModel
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre completo.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el correo.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        public string Correo { get; set; } = string.Empty;

        public string? Contrasena { get; set; }

        [Required(ErrorMessage = "Seleccione un rol.")]
        public string Rol { get; set; } = "Empleado";
    }
}
