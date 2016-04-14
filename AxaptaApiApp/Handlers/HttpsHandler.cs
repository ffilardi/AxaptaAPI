using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AxaptaApiApp.Handlers
{
    public class HttpsHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                return Task.Factory.StartNew(() =>
                {
                    return request.CreateErrorResponse(HttpStatusCode.Forbidden, "HTTPS required");
                });
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}