using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private readonly AppDbContext dbContext;

        public HotelRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Hotel> GetHotels()
        {
            return dbContext.Hotels
                .Include(hotel => hotel.HotelRooms)
                .ThenInclude(hotelRoom => hotelRoom.Room)
                .OrderBy(hotel => hotel.Id)
                .ToList();
        }
        public Hotel GetHotelById(int Id)
        {
            var hotel = dbContext.Hotels
                .Include(hotel => hotel.HotelRooms)
                .ThenInclude(hotelRoom => hotelRoom.Room)
                .Where(hotel => hotel.Id == Id)
                .OrderBy(hotel => hotel.Id)
                .FirstOrDefault();

            return hotel;
        }

        public bool AddHotel(Hotel hotel)
        {
            try
            {
                dbContext.Hotels.Add(hotel);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateHotel(Hotel hotel, List<int> newRoomIds)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var existingHotelRooms = dbContext.HotelRooms
                        .Where(hr => hr.HotelId == hotel.Id && !newRoomIds.Contains(hr.RoomId))
                        .ToList();

                    foreach (var hotelRoom in existingHotelRooms)
                    {
                        dbContext.HotelRooms.Remove(hotelRoom);
                    }

                    foreach (var roomId in newRoomIds)
                    {
                        if (!dbContext.HotelRooms.Any(hr => hr.HotelId == hotel.Id && hr.RoomId == roomId))
                        {
                            dbContext.HotelRooms.Add(new HotelRoom
                            {
                                HotelId = hotel.Id,
                                RoomId = roomId
                            });
                        }
                    }

                    dbContext.Entry(hotel).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }


        public bool DeleteHotel(Hotel hotel)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Hotels.Remove(hotel);
                    var hotelRooms = dbContext.HotelRooms.Where(hr => hr.HotelId == hotel.Id).ToList();
                    foreach (var hotelRoom in hotelRooms)
                    {
                        dbContext.HotelRooms.Remove(hotelRoom);
                    }
                    dbContext.SaveChanges();
                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }

    }
}
