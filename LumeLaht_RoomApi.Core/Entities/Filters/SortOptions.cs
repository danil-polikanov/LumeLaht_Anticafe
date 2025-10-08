using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Core_.Entities.Filters
{
    public class SortOptions
    {
        public string? SortBy { get; set; } = "name";
        public string? SortOrder { get; set; } = "asc";
    }
}
