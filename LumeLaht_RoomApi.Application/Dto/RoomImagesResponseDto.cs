using LumeLaht_RoomApi.Core_.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Application.Dto
{
    public class RoomImagesResponseDto
    {
        public Guid ImageId { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public Guid RoomId { get; set; }
    }
}
