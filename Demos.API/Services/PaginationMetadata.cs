namespace Demo.API.Services
{
    public class PaginationMetadata
    {
        public PaginationMetadata(int pageSize, int totalItems, int currentPage)
        {
            PageSize = pageSize;
            TotalItems = totalItems;
            CurrentPage = currentPage;
        }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int CurrentPage { get; set; }
    }
}
