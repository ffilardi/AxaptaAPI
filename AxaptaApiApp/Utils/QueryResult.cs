using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AxaptaApiApp.Utils
{
    public class QueryResult
    {
        [DataMember]
        public Int64 Fetched { get; set; }

        [DataMember]
        public string Table { get; set; }

        [DataMember]
        public List<Dictionary<string, object>> Rows { get; set; }

        public QueryResult()
        {
            Fetched = 0;
            Table = "";
            Rows = new List<Dictionary<string, object>>();
        }
    }
}