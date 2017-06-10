using AxaptaApiApp.QueryService;

namespace AxaptaApiApp.Utils
{
    public class QuerySettings
    {
        public string orderBy { get; set; }
        public string filterBy { get; set; }
        public string filter { get; set; }
        public int startPos { get; set; }
        public int fetch { get; set; }

        private SortOrder sort { get; set; }

        public string sortOrder
        {
            set
            {
                switch (value)
                {
                    case "asc":
                        sort = SortOrder.Ascending;
                        break;
                    case "desc":
                        sort = SortOrder.Descending;
                        break;
                    default:
                        sort = SortOrder.Ascending;
                        break;
                }
            }
        }

        public SortOrder getSortOrder()
        {
            return sort;
        }

        public QuerySettings()
        {
            orderBy = "RecId";
            sort = SortOrder.Ascending;
            filterBy = "";
            filter = "";
            startPos = 1;
            fetch = 10;
        }
    }
}
