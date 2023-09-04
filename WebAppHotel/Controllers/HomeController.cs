using Api.Models;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using WebAppHotel.Models;

namespace WebAppHotel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IHotelService hotelSrv;
        private readonly IRoomService roomSrv;
        private readonly IMapper mapper;
        public HomeController(ILogger<HomeController> logger, IMapper mapper, IHotelService hotelSrv, IRoomService roomSrv)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.hotelSrv = hotelSrv;
            this.roomSrv = roomSrv;
        }

        public IActionResult ListHotels()
        {
            var hotels = hotelSrv.GetHotels();

            var hotelsVM = hotels.Select(hotel =>
            {
                var hotelVM = mapper.Map<HotelViewModel>(hotel);
                hotelVM.RoomIds = hotel.HotelRooms?.Select(hr => hr.RoomId).ToList();
                hotelVM.SelectedRooms = hotel.HotelRooms?.Select(hr => hr.Room).ToList();

                return hotelVM;
            }).ToList();

            ViewBag.hotels = hotelsVM;
            return View();
        }


        public IActionResult AddHotel()
        {
            var rooms = roomSrv.GetRooms();
            ViewBag.Rooms = new SelectList(rooms, "Id", "Name");
            return View(new HotelViewModel());
        }

        [HttpPost]
        public IActionResult AddHotel(HotelViewModel hotel)
        {
            try
            {
                if(hotel.Name != null && hotel.RoomIds != null && hotel.Address != null && hotel.PhoneNumber != null)
                {
                    var hotelEntity = mapper.Map<Hotel>(hotel);

                    if (hotel.RoomIds != null && hotel.RoomIds.Any())
                    {
                        hotelEntity.HotelRooms = hotel.RoomIds.Select(roomId => new HotelRoom
                        {
                            HotelId = hotelEntity.Id,
                            RoomId = roomId
                        }).ToList();
                    }

                    var result = hotelSrv.AddHotel(hotelEntity);

                    if (!result)
                    {
                        throw new ApplicationException("No se pudo insertar el hotel");
                    }
                    ListHotels();
                    return RedirectToAction("ListHotels");
                }
                else
                {
                    var rooms = roomSrv.GetRooms();
                    ViewBag.Rooms = new SelectList(rooms, "Id", "Name");
                    return View();
                }
            }
            catch (ApplicationException ex)
            {
                logger.LogError(ex, "HomeController.AddHotel() - ApplicationException");
                return StatusCode(400, "Error de aplicación");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HomeController.AddHotel()");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        public IActionResult EditHotel(int Id)
        {
            var hotelEntity = hotelSrv.GetHotelById(Id);
            var hotelVM = mapper.Map<HotelViewModel>(hotelEntity);
            hotelVM.RoomIds = hotelEntity.HotelRooms?.Select(hr => hr.RoomId).ToList();
            hotelVM.SelectedRooms = hotelEntity.HotelRooms?.Select(hr => hr.Room).ToList();

            if (hotelVM == null)
            {
                return NotFound();
            }
            var rooms = roomSrv.GetRooms();
            ViewBag.Rooms = new SelectList(rooms, "Id", "Name");
            return View(hotelVM);
        }

        [HttpPost]
        public IActionResult EditHotel(HotelViewModel hotelViewModel)
        {
            if (hotelViewModel == null)
            {
                return BadRequest("El objeto hotelViewModel es nulo");
            }

            try
            {
                var existingHotel = hotelSrv.GetHotelById(hotelViewModel.Id);

                if (existingHotel == null)
                {
                    return NotFound("Hotel no encontrado");
                }

                existingHotel.Name = hotelViewModel.Name;
                existingHotel.Stars = hotelViewModel.Stars;
                existingHotel.Address = hotelViewModel.Address;
                existingHotel.PhoneNumber = hotelViewModel.PhoneNumber;

                var newRoomIds = hotelViewModel.RoomIds ?? new List<int>();

                if (hotelSrv.UpdateHotel(existingHotel, newRoomIds))
                {
                    ListHotels();
                    return RedirectToAction("ListHotels");
                }
                else
                {
                    return BadRequest("No se pudo actualizar el hotel");
                }
            }
            catch (ApplicationException ex)
            {
                logger.LogError(ex, "HomeController.EditHotel() - ApplicationException");
                return StatusCode(400, "Error de aplicación");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HomeController.EditHotel()");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        public IActionResult DeleteHotel([FromRoute] int Id)
        {
            try
            {
                var hotelEntity = hotelSrv.GetHotelById(Id);
                var result = hotelSrv.DeleteHotel(hotelEntity);
                ListHotels();
                return RedirectToAction("ListHotels");
            }
            catch (ApplicationException ex)
            {
                logger.LogError(ex, "HomeController.DeleteHotel() - ApplicationException");
                return StatusCode(400, "Error de aplicación");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "HomeController.DeleteHotel()");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}