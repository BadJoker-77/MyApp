
namespace MyApp.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalCount { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            this.AddRange(items);
        }

        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
    }
}
