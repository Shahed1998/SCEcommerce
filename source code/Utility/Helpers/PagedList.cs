using Models.Entities;

namespace Utility.Helpers
{
    public class PagedList
    {
        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public long TotalCount { get; private set; }

        public PagedList(int page, int pageSize, int total) 
        {
            PageNumber = page;
            PageSize = pageSize;
            TotalCount = total;
            TotalPages = (int)Math.Ceiling((double) total / pageSize);
        }

        #region List

        public IEnumerable<Category>? categories { get; set; }

        #endregion
    }
}
