using System;
using System.Threading.Tasks;
using System.Web.Http;
using AxaptaApiApp.Utils;
using System.Web.Http.Description;
using AxaptaApiApp.UserSessionService;
using System.Security.Claims;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace AxaptaApiApp.Controllers
{
    /// <summary>
    /// UserSession Service Controller
    /// </summary>
    public class UserInfoController : ApiController
    {
        private CallContext context = ClientFactory.CreateContext<CallContext>();

        /// <summary>
        /// Get user information
        /// </summary>
        [HttpGet]
        [Route("user")]
        [ResponseType(typeof(UserSessionInfo))]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            try
            {
                using (var client = ClientFactory.CreateClient<UserSessionServiceClient>())
                {
                    var request = await client.GetUserSessionInfoAsync(context);

                    return Ok(request.response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get current credential for authenticated user
        /// </summary>
        [HttpGet]
        [Route("user/auth")]
        [ResponseType(typeof(UserAuthenticationInfo))]
        public IHttpActionResult GetUserAuthentication()
        {
            try
            {
                var response =
                    new UserAuthenticationInfo(
                        User.Identity.IsAuthenticated,
                        User.Identity.AuthenticationType,
                        User.Identity.Name,
                        (User as ClaimsPrincipal).Claims);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    [DataContract]
    public class UserAuthenticationInfo
    {
        [DataMember]
        public bool IsAuthenticated { get; set; }

        [DataMember]
        public string AuthenticationType { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<Claim> Claims { get; set; }

        public UserAuthenticationInfo(bool isAuthenticated, string authenticationType, string name, IEnumerable<Claim> claims)
        {
            IsAuthenticated = isAuthenticated;
            AuthenticationType = authenticationType;
            Name = name;
            Claims = claims;
        }
    }
}
