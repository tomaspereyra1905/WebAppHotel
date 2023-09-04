using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class HotelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stars { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        [Display(Name = "Rooms")]
        public List<int> RoomIds { get; set; }
        public List<Room> SelectedRooms { get; set; }
    }
}