using BookingManagement___Projekt_zaliczeniowy.Abstract;
using Microsoft.AspNetCore.Mvc;
using BookingManagement___Projekt_zaliczeniowy.Models;

namespace BookingManagement___Projekt_zaliczeniowy.Controllers
{
    public class HotelController : Controller
    {

        private readonly IHotelRepo _hotel;
        private readonly IRoomRepo _room;

        public HotelController(IHotelRepo hotel,IRoomRepo room)
        {
            _hotel = hotel;
            _room = room;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddHotel(Hotel? hotel, string? temp,string? Id)
        {
            TempData["Edit"] = null;
            if (temp == "True")
            {
                TempData["Edit"] = temp;
                hotel = _hotel.GetById(hotel.Id.Replace("%2F", "/"));
            }
            ViewBag.Hotels = _hotel.GetAllHotels();
            if (hotel.Name != null)
                return View(hotel);
            return View();
        }

        public IActionResult EditHotel(string id)
        {  
            TempData["Edit"] = "True";
            return RedirectToAction("AddHotel",new {  id ,temp = TempData["Edit"]});
        }

        public IActionResult Edit(string id,Hotel newHotel)
        {
            using (var session = Session.Store.OpenSession())
            {
                var hotel = session.Query<Hotel>().Where(x => x.Id == id.Replace("%2F", "/")).First();
                hotel.Name = newHotel.Name;
                hotel.Street = newHotel.Street;
                hotel.AvgMark = newHotel.AvgMark;
                session.SaveChanges();
            }
            return RedirectToAction("AddHotel","Hotel");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Hotel hotel)
        {
            _hotel.AddHotel(hotel);
            return RedirectToAction("AddHotel", "Hotel");
        }

        
        public IActionResult Delete(string id)
        {
            id = id.Replace("%2F", "/");
            using (var session = Session.Store.OpenSession())
            {
                var rooms = session.Query<Room>().Where(x => x.HotelId == id).ToList();
                foreach(var room in rooms)
                {
                    _room.RemoveRoom(room.Id);
                }
                _hotel.RemoveHotel(id);       
                session.SaveChanges();
            }
            return RedirectToAction("AddHotel", "Hotel");
        }
    }
}
