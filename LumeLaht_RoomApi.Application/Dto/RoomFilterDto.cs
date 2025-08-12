using LumeLaht_RoomApi.Core_.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Application.Dto
{
    public class RoomFilterDto
    {
        public List<string> rooms;
        public List<string> activities;
        public int MinPrice;
        public int MaxPrice;
    }
}
