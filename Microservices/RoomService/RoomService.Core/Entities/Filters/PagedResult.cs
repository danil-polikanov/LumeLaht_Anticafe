namespace RoomService.Core.Entities.Filters
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public PaginationOptions pagination {get;set;}
    }
}
