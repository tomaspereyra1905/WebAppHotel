using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IHotelRepository
    {
        List<Hotel> GetHotels();
        Task<IEnumerable<Hotel>> GetHotelsAsync();
        Task<IEnumerable<Hotel>> GetHotelByIdAsync(int Id);
        Task<bool> AddHotelAsync(Hotel hotel);
        Hotel GetHotelById(int Id);
        bool AddHotel(Hotel hotel);
        bool UpdateHotel(Hotel hotel, List<int> newRoomIds);
        bool DeleteHotel(Hotel hotel);
    }
}
