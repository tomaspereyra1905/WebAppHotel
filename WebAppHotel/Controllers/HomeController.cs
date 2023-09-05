using Api.Models;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
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

        public async Task<IActionResult> ListHotels()
        {
            var hotels = await hotelSrv.GetHotelsAsync();

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


        public async Task<IActionResult> AddHotel()
        {
            IEnumerable rooms = await roomSrv.GetRoomsAsync();
            ViewBag.Rooms = new SelectList(rooms, "Id", "Name");
            return View(new HotelViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddHotel(HotelViewModel hotel)
        {
            try
            {
                if(ModelState.IsValid && hotel.RoomIds != null)
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

                    var result = await hotelSrv.AddHotelAsync(hotelEntity);

                    if (!result)
                    {
                        throw new ApplicationException("No se pudo insertar el hotel");
                    }
                    await ListHotels();
                    return RedirectToAction("ListHotels");
                }
                else
                {
                    IEnumerable rooms = await roomSrv.GetRoomsAsync();
                    ViewBag.Rooms = new SelectList(rooms, "Id", "Name");
                    if(hotel.RoomIds == null) ModelState.AddModelError(string.Empty, "You must select at least one room");
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

        public async Task<IActionResult> EditHotel(int Id)
        {
            var hotelEntity = (await hotelSrv.GetHotelByIdAsync(Id)).FirstOrDefault();
            var hotelVM = mapper.Map<HotelViewModel>(hotelEntity);
            hotelVM.RoomIds = hotelEntity?.HotelRooms?.Select(hr => hr.RoomId).ToList();
            hotelVM.SelectedRooms = hotelEntity?.HotelRooms?.Select(hr => hr.Room).ToList();

            if (hotelVM == null)
            {
                return NotFound();
            }
            IEnumerable rooms = await roomSrv.GetRoomsAsync();
            ViewBag.Rooms = new SelectList(rooms, "Id", "Name");
            return View(hotelVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditHotel(HotelViewModel hotelViewModel)
        {
            if (hotelViewModel == null)
            {
                return BadRequest("El objeto hotelViewModel es nulo");
            }

            try
            {
                var existingHotel = await hotelSrv.GetHotelByIdAsync(hotelViewModel.Id);

                if (existingHotel == null)
                {
                    return NotFound("Hotel no encontrado");
                }

                foreach (var hotel in existingHotel)
                {
                    hotel.Name = hotelViewModel.Name;
                    hotel.Stars = hotelViewModel.Stars;
                    hotel.Address = hotelViewModel.Address;
                    hotel.PhoneNumber = hotelViewModel.PhoneNumber;
                }

                var newRoomIds = hotelViewModel.RoomIds ?? new List<int>();
                var existingHotelEntity = existingHotel.FirstOrDefault();

                if (hotelSrv.UpdateHotel(existingHotelEntity, newRoomIds))
                {
                    await ListHotels();
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

        public async Task<IActionResult> DeleteHotel([FromRoute] int Id)
        {
            try
            {
                var hotelEntity = await hotelSrv.GetHotelByIdAsync(Id);
                var result = hotelSrv.DeleteHotel(hotelEntity.FirstOrDefault());
                await ListHotels();
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