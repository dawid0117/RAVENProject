using BookingManagement___Projekt_zaliczeniowy.Abstract;
using Microsoft.AspNetCore.Mvc;
using BookingManagement___Projekt_zaliczeniowy.Models;

namespace BookingManagement___Projekt_zaliczeniowy.Controllers
{
    public class ReservationController : Controller
    {

        private readonly IGuestRepo _guest;

        public ReservationController(IGuestRepo guest)
        {
            _guest = guest;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Reservation reservation)
        {
            reservation.GuestId = _guest.getByName(reservation.Email).Id;
            if(_guest.IsReservedAtTime(reservation.RoomId,reservation.begin,reservation.end) == false)
            {
                _guest.MakeReservation(reservation);
                using (var session = Session.Store.OpenSession())
                {
                    var guest = session.Load<Guest>(reservation.GuestId);
                    if (guest.ReservedRooms == null)
                        guest.ReservedRooms = new List<string> { reservation.Id };
                    else
                        guest.ReservedRooms.Add(reservation.Id);
                    session.SaveChanges();
                }
                return RedirectToAction("Reservation", "Room", new { error = "success" });
            }
            return RedirectToAction("Reservation", "Room", new { error = "error" });
        }

        public IActionResult Delete(string id)
        {
            id = id.Replace("%2F", "/");
            using(var session = Session.Store.OpenSession())
            {
                var guest = session.Query<Guest>().Where(x => x.ReservedRooms.Contains(id)).First();
                guest.ReservedRooms.Remove(id);
                var reservation = session.Load<Reservation>(id);
                session.Delete(reservation);
                session.SaveChanges();
            }
            return RedirectToAction("ListReservationsAdmin", "Guest");
        }

        public IActionResult Edit(string id) 
        {
            Reservation reservation;
            using(var session = Session.Store.OpenSession())
            {
                reservation = session.Load<Reservation>(id.Replace("%2F", "/"));   
            }
            return View(reservation);

        }

        [HttpPost]
        public IActionResult EditR(Reservation reservation)
        {
            using(var session = Session.Store.OpenSession())
            {
                var reser = session.Load<Reservation>(reservation.Id);
                if (_guest.IsReservedAtTime(reservation.RoomId, reservation.begin, reservation.end,reservation) == false)
                {
                    reser.begin = reservation.begin;
                    reser.end = reservation.end;
                    session.SaveChanges();
                    return RedirectToAction("Reservation", "Room", new { error = "success" });
                }
            }
            return RedirectToAction("Reservation", "Room", new { error = "error" });
        }

        public IActionResult Sort(string id)
        {
            var sortby = id;
            return RedirectToAction("ListReservationsAdmin", "Guest", new { Sort = sortby });    
        }
    }
}
