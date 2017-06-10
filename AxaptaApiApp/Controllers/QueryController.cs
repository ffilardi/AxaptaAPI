using System.Web.Http;
using AxaptaApiApp.Utils;
using System.Web.Http.Description;

namespace AxaptaApiApp.Controllers
{
    /// <summary>
    /// Controller to retrieve records from data dictionary tables
    /// </summary>
    [RoutePrefix("data")]
    public class QueryController : ApiController
    {
        /// <summary>
        /// Retrieve records from tables
        /// </summary>
        /// <param name="table">Table name</param>
        /// <param name="orderBy">Field name to order by</param>
        /// <param name="sortOrder">Sorting order (asc / desc)</param>
        /// <param name="filterBy">Field name to filter by</param>
        /// <param name="filter">Value to filter</param>
        /// <param name="startPos">Position to start retrieving records</param>
        /// <param name="fetch">Number of records to fetch</param>
        [HttpGet]
        [Route("{table:alpha}")]
        [ResponseType(typeof(QueryResult))]
        public IHttpActionResult GetQuery(
            string table,
            string orderBy = "RecId",
            string sortOrder = "asc",
            string filterBy = "",
            string filter = "",
            int startPos = 1,
            int fetch = 10)
        {
            QuerySettings settings = new QuerySettings()
            {
                orderBy = orderBy,
                sortOrder = sortOrder,
                filterBy = filterBy,
                filter = filter,
                startPos = startPos,
                fetch = fetch
            };

            return new QueryRequest(Request, table, settings);
        }

        /// <summary>
        /// Retrieve records from tables
        /// </summary>
        /// <param name="table">Table name</param>
        /// <param name="settings"></param>
        [HttpPost]
        [Route("{table:alpha}")]
        [ResponseType(typeof(QueryResult))]
        public IHttpActionResult PostQuery([FromUri] string table, [FromBody] QuerySettings settings)
        {
            if (settings == null)
            {
                settings = new QuerySettings();
            }

            return new QueryRequest(Request, table, settings);
        }
    }
}
