using System;
using System.Web;

namespace Skynetz.Web.Services
{
    /// <summary>
    /// Lê ou cria o identificador anônimo persistido no cookie do navegador.
    /// Esta classe pertence à camada Web porque depende de HTTP.
    /// </summary>
    public class VisitanteCookieProvider
    {
        private const string NomeCookie = "Skynetz.VisitanteId";
        private const int DuracaoCookieDias = 365;

        public Guid ObterOuCriar(HttpRequest request, HttpResponse response)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (response == null)
                throw new ArgumentNullException("response");

            var cookieRecebido = request.Cookies[NomeCookie];
            Guid visitanteId;

            if (cookieRecebido != null &&
                Guid.TryParse(cookieRecebido.Value, out visitanteId) &&
                visitanteId != Guid.Empty)
            {
                return visitanteId;
            }

            visitanteId = Guid.NewGuid();

            var caminhoCookie = request.ApplicationPath;
            if (string.IsNullOrWhiteSpace(caminhoCookie))
                caminhoCookie = "/";

            var cookie = new HttpCookie(NomeCookie, visitanteId.ToString("D"))
            {
                HttpOnly = true,
                Secure = request.IsSecureConnection,
                SameSite = SameSiteMode.Lax,
                Path = caminhoCookie,
                Expires = DateTime.UtcNow.AddDays(DuracaoCookieDias)
            };

            // Set substitui um cookie anterior com o mesmo nome nesta resposta.
            response.Cookies.Set(cookie);

            // O ID recém-gerado também é usado nesta própria requisição.
            return visitanteId;
        }
    }
}
