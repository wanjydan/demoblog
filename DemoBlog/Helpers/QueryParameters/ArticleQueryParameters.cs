namespace DemoBlog.Helpers.QueryParameters
{
    public class ArticleQueryParameters
    {
        private const int MaxPageSize = 20;
        private int _pageSize = 5;
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string Category { get; set; }
        public string Search { get; set; }
        public string OrderBy { get; set; } = "Date desc";
        public string Fields { get; set; } = "id,title,slug";
    }
}