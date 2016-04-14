using System;
using System.Threading.Tasks;
using System.Web.Http;
using AxaptaApiApp.Utils;
using System.Web.Http.Description;
using AxaptaApiApp.UserSessionService;

namespace AxaptaApiApp.Controllers
{
    /// <summary>
    /// UserSession Service Controller
    /// </summary>
    public class UserSessionController : ApiController
    {
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
                    return Ok(
                        await client.GetUserSessionInfoAsync(new CallContext()));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
