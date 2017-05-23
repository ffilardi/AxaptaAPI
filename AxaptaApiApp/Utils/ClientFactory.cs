using System;
using System.Web;

namespace AxaptaApiApp.Utils
{
    /// <summary>
    /// Factory to create AX service clients
    /// </summary>
    public static class ClientFactory
    {
        /// <summary>
        /// Creates service clients based on the Generic type TClass
        /// </summary>
        /// <typeparam name="TClass">The generic representation to the class</typeparam>
        /// <returns>The service class based on the generic type</returns>
        public static TClass CreateClient<TClass>() where TClass : new()
        {
            try
            {
                dynamic client = new TClass();
                client.ClientCredentials.Windows.ClientCredential = ServiceConfig.NetworkCredential();

                return client;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static TClass CreateContext<TClass>() where TClass : new()
        {
            try
            {
                dynamic context = new TClass();
                var company = HttpContext.Current.Request.QueryString.Get("company");

                if (!String.IsNullOrEmpty(company))
                {
                    context.Company = company;
                }

                if (ServiceConfig.GetAuthenticationMode() == ServiceConfig.AuthenticationMode.ThirdPartyProvider)
                {
                    context.LogonAsUser = String.Format("{0}\\{1}",
                            HttpContext.Current.User.Identity.AuthenticationType,
                            HttpContext.Current.User.Identity.Name);
                }

                return context;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}