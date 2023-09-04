using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Hotel : BaseEntity
    {
        public string Name { get; set; }
        public int Stars { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<HotelRoom> HotelRooms { get; set; }
    }
}
