using System;
using System.Net;
using System.Configuration;
using System.Text;
using System.Threading;
using AxaptaApiApp.Utils;
using System.Net.Http;

namespace AxaptaApiApp
{
    public static class ServiceConfig
    {
        public enum AuthenticationMode
        {
            ActiveDirectory,
            ActiveDirectorySingleUser,
            ThirdPartyProvider,
            Impersonate,
            NotDefined
        };

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

        public static AuthenticationMode GetAuthenticationMode()
        {
            int mode = 0;
            if (Int32.TryParse(ConfigurationManager.AppSettings["API_AUTH_MODE"], out mode))
            {
                switch (mode)
                {
                    case 1:
                        return AuthenticationMode.ActiveDirectory;
                    case 2:
                        return AuthenticationMode.ActiveDirectorySingleUser;
                    case 3:
                        return AuthenticationMode.ThirdPartyProvider;
                    case 4:
                        return AuthenticationMode.Impersonate;
                    default:
                        return AuthenticationMode.NotDefined;
                }
            }
            else
            {
                return AuthenticationMode.NotDefined;
            }
        }

        public static BasicAuthIdentity ParseUserCredential()
        {
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["API_AUTH_USER_DOMAIN"])
                || String.IsNullOrEmpty(ConfigurationManager.AppSettings["API_AUTH_USER_NAME"])
                || String.IsNullOrEmpty(ConfigurationManager.AppSettings["API_AUTH_USER_PASSWORD"]))
            {
                return null;
            }

            return new BasicAuthIdentity(
                ConfigurationManager.AppSettings["API_AUTH_USER_DOMAIN"],
                ConfigurationManager.AppSettings["API_AUTH_USER_NAME"],
                ConfigurationManager.AppSettings["API_AUTH_USER_PASSWORD"]
            );
        }

        public static BasicAuthIdentity ParseAuthorizationHeader(HttpRequestMessage request)
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

        public static bool OnAuthorizeUser(BasicAuthIdentity identity)
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