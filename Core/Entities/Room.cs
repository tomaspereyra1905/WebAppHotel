using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Room : BaseEntity
    {
        public string Name { get; set; }
        public int MaxGuests { get; set; }
        public ICollection<HotelRoom> HotelRooms { get; set; }
    }
}
