using System;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using AxaptaApiApp.Utils;
using AxaptaApiApp.Filters;
using AxaptaApiApp.MetadataService;

namespace AxaptaApiApp.Controllers
{
    public class ExtendedDataTypeController : ApiController
    {
        /// <summary>
        /// Gets details for one extended data type
        /// </summary>
        /// <param name="name">Extended data type object name</param>
        [HttpGet]
        [Route("datatype/{name:alpha}")]
        [ResponseType(typeof(EdtMetadata[]))]
        [CacheFilter]
        public async Task<IHttpActionResult> GetExtendedDataTypeMetadataByName([FromUri] string name)
        {
            try
            {
                using (var client = ClientFactory.CreateClient<AxMetadataServiceClient>())
                {
                    var response = await client.GetExtendedDataTypeMetadataByNameAsync(new string[] { name });

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets details for one extended data type
        /// </summary>
        /// <param name="id">Extended data type object identification</param>
        [HttpGet]
        [Route("datatype/{id:int}")]
        [ResponseType(typeof(EdtMetadata[]))]
        [CacheFilter]
        public async Task<IHttpActionResult> GetEDTById([FromUri] int id)
        {
            try
            {
                using (var client = ClientFactory.CreateClient<AxMetadataServiceClient>())
                {
                    var response = await client.GetExtendedDataTypeMetadataByIdAsync(new int[] { id });

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}