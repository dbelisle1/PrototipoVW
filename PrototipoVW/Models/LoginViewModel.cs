using System.ComponentModel.DataAnnotations;

namespace PrototipoVW.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingrese el correo.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la contraseña.")]
        public string Contrasena { get; set; } = string.Empty;
    }
}
