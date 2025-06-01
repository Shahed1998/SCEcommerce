using Models.BusinessEntities;

namespace Utility.Helpers
{
    public class PagedList
    {
        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public long TotalCount { get; private set; }

        public int FirstItemOnPage { get; private set; }

        public PagedList(int page, int pageSize, int total) 
        {
            PageNumber = page;
            PageSize = pageSize;
            TotalCount = total;
            TotalPages = (int)Math.Ceiling((double) total / pageSize);
            FirstItemOnPage = pageSize * (page - 1) + 1;
        }

        #region List

        public IEnumerable<CategoryVM>? categories { get; set; } 
        public IEnumerable<ProductVM>? products { get; set; } 

        #endregion
    }
}
