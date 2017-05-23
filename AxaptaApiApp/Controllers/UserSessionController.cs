using System;
using System.Threading.Tasks;
using System.Web.Http;
using AxaptaApiApp.Utils;
using System.Web.Http.Description;
using AxaptaApiApp.UserSessionService;
using System.Security.Claims;
using System.Linq;

namespace AxaptaApiApp.Controllers
{
    /// <summary>
    /// UserSession Service Controller
    /// </summary>
    public class UserSessionController : ApiController
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
                    return Ok(await client.GetUserSessionInfoAsync(context));
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
        [Route("auth")]
        [ResponseType(typeof(object))]
        public IHttpActionResult GetUserAuthentication()
        {
            try
            {
                return Ok(
                    new
                    {
                        IsAuthenticated = this.User.Identity.IsAuthenticated.ToString(),
                        AuthenticationType = this.User.Identity.AuthenticationType,
                        Name = this.User.Identity.Name,
                        Claims = (this.User as ClaimsPrincipal).Claims.Select(c => new { Type = c.Type, Value = c.Value })
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
