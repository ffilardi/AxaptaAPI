using System;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Security.Principal;
using System.Security.Claims;

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
                else if (ServiceConfig.GetAuthenticationMode() == ServiceConfig.AuthenticationMode.Impersonate)
                {
                    string domain = HttpContext.Current.User.Identity.AuthenticationType;
                    string username = "";
                    ClaimsPrincipal claimsPrincipal = HttpContext.Current.User as ClaimsPrincipal;
                    string email = claimsPrincipal.FindFirst(ClaimTypes.Upn) != null ? claimsPrincipal.FindFirst(ClaimTypes.Upn).Value : claimsPrincipal.FindFirst(ClaimTypes.Email).Value;

                    //aad is Azure Active Directory - if aad is used then use the internal network domain name from the app settings
                    //Other possible values are Google, Facebook, Twitter - for those we will pass direct to AX as a claims user
                    if (String.Equals(domain, "aad"))
                    {
                        domain = ConfigurationManager.AppSettings["API_AUTH_USER_DOMAIN"];
                        MailAddress addr = new MailAddress(email); //takes username@mydomain.com
                        username = addr.User;                      //returns username
                    }
                    context.LogonAsUser = String.Format("{0}\\{1}", domain, username);
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