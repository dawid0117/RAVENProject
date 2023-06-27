namespace BookingManagement___Projekt_zaliczeniowy.Models
{
    public class Reservation
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string RoomId { get; set; }
        public string GuestId { get; set; }
        public DateTime begin { get; set; }
        public DateTime end { get; set; }
    }
}
