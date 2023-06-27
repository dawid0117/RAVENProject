﻿namespace BookingManagement___Projekt_zaliczeniowy.Models
{
    public class Guest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> ReservedRooms { get; set; }
    }
}
