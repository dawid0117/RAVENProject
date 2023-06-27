using BookingManagement___Projekt_zaliczeniowy.Abstract;
using BookingManagement___Projekt_zaliczeniowy.Index;
using BookingManagement___Projekt_zaliczeniowy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingManagement___Projekt_zaliczeniowy.Controllers
{
    [Authorize]
    public class GuestController : Controller
    {

        private readonly IGuestRepo _guest;

        public GuestController(IGuestRepo guest)
        {
            _guest = guest;
        }


        public IActionResult Index()
        {
            var guest = _guest.getByName(User.Identity.Name);
            if (guest != null)
            {
                TempData["View"] = "guest";
                return View(guest);
            }
            else
                return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Guest guest)
        {
            _guest.AddGuest(guest);
            return RedirectToAction("Reservation","Room",new { isReserved = "reserved"});
        }

        [HttpGet]
        public IActionResult ListReservations()
        {
            List<Reservation> reservations;
            using(var session = Session.Store.OpenSession())
            {
                reservations = session.Query<Reservation>()
                    .Where(x => x.Email == User.Identity.Name)
                    .ToList();

            }
            ViewBag.Reservations = reservations;
            return View();
        }
    }
}
