using Core.Entities;

namespace Api.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxGuests { get; set; }
        public ICollection<HotelRoom> HotelRooms { get; set; }
    }
}
