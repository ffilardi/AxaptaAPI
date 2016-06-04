using System;
using System.Configuration;

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

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["API_LOGIN_COMPANY"]) &&
                    !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["API_LOGIN_COMPANY"]))
                {
                    context.Company = ConfigurationManager.AppSettings["API_LOGIN_COMPANY"];
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