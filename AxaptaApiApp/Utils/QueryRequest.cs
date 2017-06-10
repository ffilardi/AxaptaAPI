using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Threading;
using AxaptaApiApp.QueryService;
using System.Collections.Generic;
using System.Data;

namespace AxaptaApiApp.Utils
{
    public class QueryRequest : IHttpActionResult
    {
        HttpRequestMessage _request;
        QuerySettings _settings;
        string _table;

        public QueryRequest(HttpRequestMessage request, string table, QuerySettings settings)
        {
            _request = request;
            _table = table;
            _settings = settings;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            try
            {
                QueryMetadata query = createQuery(_table, _settings);

                Paging paging = new PositionBasedPaging()
                {
                    StartingPosition = _settings.startPos,
                    NumberOfRecordsToFetch = _settings.fetch
                };

                using (var client = ClientFactory.CreateClient<QueryServiceClient>())
                {
                    ExecuteQueryRequest queryRequest = new ExecuteQueryRequest(query, paging);
                    ExecuteQueryResponse queryResponse = await client.ExecuteQueryAsync(queryRequest);

                    response = createQueryResponse(_request, queryResponse.ExecuteQueryResult);
                }
            }
            catch (Exception ex)
            {
                response = _request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return response;
        }

        private HttpResponseMessage createQueryResponse(HttpRequestMessage request, DataSet dataSet)
        {
            HttpResponseMessage response;

            if (dataSet == null)
            {
                response = request.CreateErrorResponse(HttpStatusCode.NoContent, "No data or invalid dataset.");
            }
            else
            {
                QueryResult queryResult = new QueryResult();
                Dictionary<string, object> result;

                foreach (DataTable table in dataSet.Tables)
                {
                    queryResult.Fetched = table.Rows.Count;
                    queryResult.Table = table.TableName;

                    foreach (DataRow row in table.Rows)
                    {
                        result = new Dictionary<string, object>();

                        foreach (DataColumn column in table.Columns)
                        {
                            result.Add(column.ColumnName, row[column]);
                        }

                        queryResult.Rows.Add(result);
                    }
                }

                response = request.CreateResponse(HttpStatusCode.OK, queryResult);
            }

            return response;
        }

        private QueryMetadata createQuery(string table, QuerySettings settings)
        {
            QueryDataSourceMetadata datasource = new QueryDataSourceMetadata();
            QueryMetadata query = new QueryMetadata();

            Paging paging = new PositionBasedPaging()
            {
                StartingPosition = settings.startPos,
                NumberOfRecordsToFetch = settings.fetch
            };

            query.DataSources = new QueryDataSourceMetadata[1];

            datasource.Name = table;
            datasource.Table = table;
            datasource.Enabled = true;
            datasource.DynamicFieldList = true;
            datasource.OrderMode = OrderMode.OrderBy;

            if (settings.filterBy != "" && settings.filter != "")
            {
                datasource.Ranges = new QueryRangeMetadata[]
                {
                    new QueryDataRangeMetadata()
                    {
                        TableName = table,
                        FieldName = settings.filterBy,
                        Value = settings.filter,
                        Enabled = true
                    }
                };
            }

            query.DataSources[0] = datasource;

            query.OrderByFields = new QueryDataOrderByMetadata[]
            {
                new QueryDataOrderByMetadata()
                {
                    DataSource = table,
                    FieldName = settings.orderBy,
                    SortOrder = settings.getSortOrder()
                }
            };

            return query;
        }
    }
}
