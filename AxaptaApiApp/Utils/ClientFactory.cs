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
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}