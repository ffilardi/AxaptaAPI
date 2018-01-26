using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using AxaptaApiApp.Utils;

namespace AxaptaApiApp.Handlers
{
    public class BasicAuthHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            BasicAuthIdentity identity = null;

            switch (ServiceConfig.GetAuthenticationMode())
            {
                case ServiceConfig.AuthenticationMode.ActiveDirectory:
                    identity = ServiceConfig.ParseAuthorizationHeader(request);
                    break;
                case ServiceConfig.AuthenticationMode.ActiveDirectorySingleUser:
                case ServiceConfig.AuthenticationMode.ThirdPartyProvider:
                case ServiceConfig.AuthenticationMode.Impersonate:
                    identity = ServiceConfig.ParseUserCredential();
                    break;
            }

            if (identity == null || !ServiceConfig.OnAuthorizeUser(identity))
            {
                return Task.Factory.StartNew(() =>
                {
                    return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Missing or invalid authentication credential");
                });
            }

            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            return base.SendAsync(request, cancellationToken);
        }
    }
}