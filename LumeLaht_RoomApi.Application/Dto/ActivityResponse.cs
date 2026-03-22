using LumeLaht_RoomApi.Core_.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Application.Dto
{
    public class ActivityResponse
    {
        public Guid ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
