namespace PrototipoVWApi.Models
{
    public class UsuarioUpdateRequest
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? Contrasena { get; set; }
        public string Rol { get; set; } = string.Empty;
    }
}
