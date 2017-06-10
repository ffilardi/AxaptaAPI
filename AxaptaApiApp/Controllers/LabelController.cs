using System;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using AxaptaApiApp.Utils;
using AxaptaApiApp.Filters;
using AxaptaApiApp.MetadataService;
using System.Runtime.Serialization;

namespace AxaptaApiApp.Controllers
{
    public class LabelController : ApiController
    {
        /// <summary>
        /// Get the translated label
        /// </summary>
        /// <param name="language">Language and country code</param>
        /// <param name="label">Label code starting with @</param>
        [HttpGet]
        [Route("label/{language}/{label}")]
        [ResponseType(typeof(LabelTranslateMetadata))]
        [CacheFilter]
        public async Task<IHttpActionResult> GetLabel([FromUri] string language, [FromUri] string label)
        {
            try
            {
                var response = await FromLabel(language, new string[] { label });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get the translated label list
        /// </summary>
        /// <param name="language">Language and country code</param>
        /// <param name="labels">Array of label codes starting with @</param>
        [HttpPost]
        [Route("label/{language}")]
        [ResponseType(typeof(LabelTranslateMetadata))]
        public async Task<IHttpActionResult> PostLabel([FromUri] string language, [FromBody] string[] labels)
        {
            try
            {
                var response = await FromLabel(language, labels);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<LabelTranslateMetadata> FromLabel(string language, string[] labels)
        {
            try
            {
                using (var client = ClientFactory.CreateClient<AxMetadataServiceClient>())
                {
                    var response = new LabelTranslateMetadata(language,
                        await client.GetLabelMetadataForLanguageByIdAsync(language, labels));

                    return response;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    [DataContract]
    public class LabelTranslateMetadata
    {
        [DataMember]
        public string Language { get; set; }

        [DataMember]
        LabelMetadata[] LabelMetadata { get; set; }

        public LabelTranslateMetadata(string language, LabelMetadata[] labelMetadata)
        {
            Language = language;
            LabelMetadata = labelMetadata;
        }
    }
}