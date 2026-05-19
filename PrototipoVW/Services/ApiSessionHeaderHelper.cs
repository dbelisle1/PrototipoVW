namespace PrototipoVW.Services
{
    public static class ApiSessionHeaderHelper
    {
        public static void AddSessionHeaders(this HttpRequestMessage request, ISession session)
        {
            var idUsuario = session.GetInt32("IdUsuario");
            var rol = session.GetString("Rol");

            if (idUsuario != null && !string.IsNullOrWhiteSpace(rol))
            {
                request.Headers.Add("X-User-Id", idUsuario.Value.ToString());
                request.Headers.Add("X-User-Role", rol);
            }
        }
    }
}
