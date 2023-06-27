using BookingManagement___Projekt_zaliczeniowy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookingManagement___Projekt_zaliczeniowy.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}