namespace ClientAngular.Common
{
    public class PaginationModelBase
    {
        public int StartIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public long TotalRecords { get; set; }
        public string FilterFormId { get; set; }
        public string GridContainerId { get; set; }
        public int GridPageIndex { get; set; }
    }
}
