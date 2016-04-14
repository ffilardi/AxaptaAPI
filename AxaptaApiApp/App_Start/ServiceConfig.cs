using System;
using System.Net;
using System.Configuration;
using System.Text;
using System.Threading;
using AxaptaApiApp.Utils;

namespace AxaptaApiApp
{
    public static class ServiceConfig
    {
        public static NetworkCredential NetworkCredential()
        {
            var identity = Thread.CurrentPrincipal.Identity as BasicAuthIdentity;

            if (identity != null && identity.IsAuthenticated)
            {
                return new NetworkCredential(
                    identity.Name,
                    identity.Password,
                    identity.Domain
                );
            }

            return null;
        }

        public static bool SingleUserMode()
        {
            if (ConfigurationManager.AppSettings["API_CONFIG_SINGLEUSER"].Equals("True", StringComparison.OrdinalIgnoreCase)
                || ConfigurationManager.AppSettings["API_CONFIG_SINGLEUSER"].Equals("Yes", StringComparison.OrdinalIgnoreCase)
                || ConfigurationManager.AppSettings["API_CONFIG_SINGLEUSER"].Equals("1"))
            {
                return true;
            }

            return false;
        }

        public static BasicAuthIdentity ParseAuthorizationSettings()
        {
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["API_LOGIN_DOMAIN"])
                || String.IsNullOrEmpty(ConfigurationManager.AppSettings["API_LOGIN_NAME"])
                || String.IsNullOrEmpty(ConfigurationManager.AppSettings["API_LOGIN_PASSWORD"]))
            {
                return null;
            }

            return new BasicAuthIdentity(
                ConfigurationManager.AppSettings["API_LOGIN_DOMAIN"],
                ConfigurationManager.AppSettings["API_LOGIN_NAME"],
                ConfigurationManager.AppSettings["API_LOGIN_PASSWORD"]
            );
        }
    }
}