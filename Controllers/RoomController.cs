using Microsoft.AspNetCore.Mvc;
using BookingManagement___Projekt_zaliczeniowy.Models;
using BookingManagement___Projekt_zaliczeniowy.Abstract;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookingManagement___Projekt_zaliczeniowy.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomRepo _room;
        private readonly IHotelRepo _hotel;
        private readonly IGuestRepo _guest;

        public RoomController(IRoomRepo room, IHotelRepo hotel, IGuestRepo guest)
        {
            _room = room;
            _hotel = hotel;
            _guest = guest;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Room? room,int id)
        {
            List<Room> rooms;
            if(id == 1)
                rooms = _room.GetAllRooms().Take(6).ToList();
            else 
                rooms = _room.GetAllRooms().Skip(6*(id-1)).Take(6).ToList();
            ViewBag.Hotels = _hotel.GetAllHotels().Select(x => new {x.Id,x.Name});
            List<Room> filteredRooms;
            if (room.Name != null || (room.HotelId != null && room.HotelId != "Wybierz hotel") || room.People != 0)
            {
                using (var session = Session.Store.OpenSession())
                {
                    if (room.HotelId != null && room.HotelId == "Wybierz hotel")
                    {
                        filteredRooms = session.Query<Room>()
                            .Search(x => x.Name, $"{room.Name}")
                            .Search(x => x.People, $"{room.People}", options: SearchOptions.Or)
                            .ToList();
                    }
                    else
                    {
                        filteredRooms = session.Query<Room>()
                           .Search(x => x.Name, $"{room.Name}")
                           .Search(x => x.People, $"{room.People}")
                           .Search(x => x.HotelId, $"{room.HotelId}", options: SearchOptions.And)
                           .ToList();
                    }
                    if(room.People <=0 && room.Name == null)
                    {
                        filteredRooms = session.Query<Room>()
                          .Search(x => x.HotelId, $"{room.HotelId}")
                          .ToList();
                    }
                    if(room.Name != null && (room.HotelId != null && room.HotelId != "Wybierz hotel") && room.People > 0)
                    {
                        filteredRooms = session.Query<Room>()
                          .Search(x => x.Name, $"{room.Name}")
                          .Search(x => x.People, $"{room.People}", options: SearchOptions.And)
                          .Search(x => x.HotelId, $"{room.HotelId}", options: SearchOptions.And)
                          .ToList();
                    }
                    ViewBag.Rooms = filteredRooms.Take(6).ToList();
                }
            }
            else
                ViewBag.Rooms = rooms;

            //if (room.People > 0)
            //    rooms = rooms.Where(x => x.People == room.People).ToList();

            //if (room.Name != null)
            //    rooms = rooms.Where(x => x.Name.ToLower().Contains(room.Name.ToLower())).ToList();

            //if (room.HotelId != null && room.HotelId != "Wybierz hotel")
            //    rooms = rooms.Where(x => x.HotelId == room.HotelId).ToList();

           

            return View();
        }

        [HttpGet]
        public IActionResult AddRoom()
        {
            ViewBag.Hotels = (_hotel.GetAllHotels().Select(x => new { x.Name,x.Id}));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoom(Room room)
        {
            _room.AddRoom(room);
            _hotel.AddRoom(room.Id,room.HotelId);
            //var path = $@"C:\Users\\Lisek$\Pictures\Cyberpunk 2077\{newRoom.Photo}";
            //var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            //session.Advanced.Attachments.Store(newRoom.Id, newRoom.Photo, fs, "image/png");
            //var attach = Session.Store.OpenSession().Advanced.Attachments.Get(room.Id, room.HotelId).;
            return RedirectToAction("AddRoom", "Room");
        }

        public IActionResult Delete(string id)
        {
            id = id.Replace("%2F", "/");
            var room = _room.GetRoomById(id);
            using(var session = Session.Store.OpenSession())
            {
                var hotel = session.Query<Hotel>().Where(x => x.Id == room.HotelId).First();
                if(hotel.Rooms != null && hotel.Rooms.Any())
                {
                    hotel.Rooms.Remove(id);
                    session.SaveChanges();
                }
            }
            _room.RemoveRoom(id);
            return RedirectToAction("Index", "Room");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var room = _room.GetRoomById(id.Replace("%2F","/"));
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRoom(string id,Room? newRoom)
        {
            using(var session = Session.Store.OpenSession())
            {
                var room = session.Query<Room>().Where(x => x.Id == id.Replace("%2F","/")).First();
                room.Space = newRoom.Space;
                room.Name = newRoom.Name;
                room.Description= newRoom.Description;
                room.People = newRoom.People;
                session.SaveChanges();
            }
            return RedirectToAction("Index", "Room");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Reservation(string id,string isReserved,string error)
        {
            Room room;
            var guest = _guest.getByName(User.Identity.Name);
            if (guest != null)
                TempData["isReserved"] = "reserved";
            else if(isReserved != null)
                TempData["isReserved"] = isReserved;
            if (id != null)
            {
                room = _room.GetRoomById(id.Replace("%2F", "/"));
                return View(room);
            }
            if (error != null && error == "success")
                return RedirectToAction("Success", "Room");
            else if(error != null && error == "error")
                return RedirectToAction("Error", "Room");
            return RedirectToAction("Index","Room");
        }

    }
}
