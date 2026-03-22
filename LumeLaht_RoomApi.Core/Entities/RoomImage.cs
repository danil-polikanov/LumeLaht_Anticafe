using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Core_.Entities
{
    public class RoomImage
    {
        [Key]
        public Guid ImageId { get; set; }

        [Required]
        public string Url { get; set; }

        public bool IsMain { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}
