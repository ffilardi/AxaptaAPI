using System;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
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

            if (ServiceConfig.SingleUserMode())
            {
                identity = ServiceConfig.ParseAuthorizationSettings();
            }
            else
            {
                identity = ParseAuthorizationHeader(request);
            }

            if (identity == null || !OnAuthorizeUser(identity))
            {
                return Task.Factory.StartNew(() =>
                {
                    return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Missing or Invalid Authentication");
                });
            }

            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            return base.SendAsync(request, cancellationToken);
        }

        protected virtual BasicAuthIdentity ParseAuthorizationHeader(HttpRequestMessage request)
        {
            string authParameter = null;
            var authHeader = request.Headers.Authorization;

            if (authHeader != null
                && authHeader.Parameter != null
                && authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                authParameter = Encoding.Default.GetString(Convert.FromBase64String(authHeader.Parameter));

                var credential = authParameter.Split(new char[] { '\\', ':' });

                if (credential.Length == 3)
                {
                    return new BasicAuthIdentity(credential[0], credential[1], credential[2]);
                }
            }

            return null;
        }

        protected virtual bool OnAuthorizeUser(BasicAuthIdentity identity)
        {
            if (string.IsNullOrEmpty(identity.Domain)
                || string.IsNullOrEmpty(identity.Name)
                || identity.Password.Length == 0)
            {
                return false;
            }

            return true;
        }
    }
}