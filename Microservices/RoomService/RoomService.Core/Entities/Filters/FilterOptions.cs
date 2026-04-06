namespace RoomService.Core.Entities.Filters
{
    public class FilterOptions
    {
        public string? Search { get; set; }
        public Dictionary<string, object?> Filters { get; set; } = new();
        public SortOptions SortOptions { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } =3;
    }
}
