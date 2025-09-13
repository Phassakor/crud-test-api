namespace crudActivity.Dtos
{
    public record PagedResult<T>(int Page, int PageSize, long Total, IReadOnlyList<T> Items);

    public class ActivityQuery
    {
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; } = "published";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
