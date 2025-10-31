using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Core_.Entities.Filters
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public PaginationOptions pagination {get;set;}
    }
}
