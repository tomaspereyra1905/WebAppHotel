using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public bool AddHotel(Hotel hotel)
        {
            return _hotelRepository.AddHotel(hotel);
        }

        public bool DeleteHotel(Hotel hotel)
        {
            return _hotelRepository.DeleteHotel(hotel);
        }

        public Hotel GetHotelById(int Id)
        {
            return _hotelRepository.GetHotelById(Id);
        }

        public List<Hotel> GetHotels()
        {
            return _hotelRepository.GetHotels();
        }

        public bool UpdateHotel(Hotel hotel, List<int> newRoomIds)
        {
            return _hotelRepository.UpdateHotel(hotel, newRoomIds);
        }
    }
}
