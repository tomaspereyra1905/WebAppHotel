using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IHotelService
    {
        List<Hotel> GetHotels();
        Hotel GetHotelById(int Id);
        bool AddHotel(Hotel hotel);
        bool UpdateHotel(Hotel hotel, List<int> newRoomIds);
        bool DeleteHotel(Hotel hotel);
    }
}
