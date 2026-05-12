namespace MetricsHub.Application.Pagination
{
    public class PaginatedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = [];
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasPrevious => PageNumber > 1 && Items.Any();
        public bool HasNext => PageNumber < TotalPages;

        public static PaginatedResult<T> Create(
            List<T> items,
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            return new PaginatedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
