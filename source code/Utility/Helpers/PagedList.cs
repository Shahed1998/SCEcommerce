namespace Utility.Helpers
{
    public class PagedList<T> : List<T> where T : class
    {
        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public long TotalCount { get; private set; }

        public PagedList(IEnumerable<T> source, int page, int pageSize, int total) 
        {
            PageNumber = page;
            PageSize = pageSize;
            TotalCount = total;
            TotalPages = (int)Math.Ceiling((double) total / pageSize);
            this.AddRange(source);
        }
    }
}
