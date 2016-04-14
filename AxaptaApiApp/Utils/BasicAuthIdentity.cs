using System.Security;
using System.Security.Principal;

namespace AxaptaApiApp.Utils
{
    /// <summary>
    /// Basic authentication identity extension
    /// </summary>
    public class BasicAuthIdentity : GenericIdentity
    {
        /// <summary>
        /// Domain name as string
        /// </summary>
        public string Domain { get; private set; }
        /// <summary>
        /// User password as SecureString object
        /// </summary>
        public SecureString Password { get; private set; }

        /// <summary>
        /// Starts a new instance of BasicAuthenticationIdentity class
        /// </summary>
        /// <param name="domain">Domain name as string</param>
        /// <param name="name">User name as string</param>
        /// <param name="password">User password as string</param>
        public BasicAuthIdentity(string domain, string name, string password)
            : base(name, "Basic")
        {
            this.Domain = domain;
            this.Password = SetSecurePassword(password);
        }

        /// <summary>
        /// Converts a string password into a SecureString object
        /// </summary>
        /// <param name="password">User password as string</param>
        /// <returns>SecureString object containing the password</returns>
        private SecureString SetSecurePassword(string password)
        {
            var secureString = new SecureString();

            foreach (char c in password.ToCharArray())
                secureString.AppendChar(c);

            return secureString;
        }
    }
}