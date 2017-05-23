using System;
using System.Threading.Tasks;
using System.Web.Http;
using AxaptaApiApp.Utils;
using System.Web.Http.Description;
using AxaptaApiApp.ExpenseService;
using AxaptaApiApp.Filters;

namespace AxaptaApiApp.Controllers
{
    public class ExpenseController : ApiController
    {
        private CallContext context = ClientFactory.CreateContext<CallContext>();

        /// <summary>
        /// Get expense details
        /// </summary>
        [HttpGet]
        [Route("expense/{exp}")]
        [ResponseType(typeof(TrvExpenseReportContract))]
        [CacheFilter]
        public async Task<IHttpActionResult> ReadExpense([FromUri] string exp)
        {
            try
            {
                using (var client = ClientFactory.CreateClient<TrvExpenseReportCustomServiceClient>())
                {
                    var request = await client.readExpenseHeaderAsync(context, exp);

                    return Ok(request.response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Post a new expense report
        /// </summary>
        [HttpPost]
        [Route("expense")]
        [ResponseType(typeof(TrvExpenseReportContract))]
        public async Task<IHttpActionResult> CreateExpense([FromBody] TrvExpenseReportContract expenseReport)
        {
            try
            {
                using (var client = ClientFactory.CreateClient<TrvExpenseReportCustomServiceClient>())
                {
                    var request = await client.createAsync(context, expenseReport);

                    return Ok(request.response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing expense report
        /// </summary>
        [HttpPut]
        [Route("expense/{exp}")]
        [ResponseType(typeof(TrvExpenseReportContract))]
        public async Task<IHttpActionResult> UpdateExpense([FromUri] string exp, [FromBody] TrvExpenseReportContract expenseReport)
        {
            try
            {
                if (!String.Equals(expenseReport.ExpNumber, exp))
                {
                    return BadRequest("The document doesn't match with the expense identification");
                }

                using (var client = ClientFactory.CreateClient<TrvExpenseReportCustomServiceClient>())
                {
                    var request = await client.updateAsync(context, expenseReport);

                    return Ok(request.response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete an existing expense report
        /// </summary>
        [HttpDelete]
        [Route("expense/{exp}")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> DeleteExpense([FromUri] string exp)
        {
            try
            {
                using (var client = ClientFactory.CreateClient<TrvExpenseReportCustomServiceClient>())
                {
                    var request = await client.deleteAsync(context, new string[] { exp });

                    return Ok(request.response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Submit expense
        /// </summary>
        [HttpPost]
        [Route("expense/{exp}/submit")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> SubmitExpenses([FromUri] string exp, [FromBody] string comment)
        {
            try
            {
                using (var client = ClientFactory.CreateClient<TrvExpenseReportCustomServiceClient>())
                {
                    var request = await client.submitAsync(context, exp, comment);

                    return Ok(request.response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
