namespace BookingManagement___Projekt_zaliczeniowy.Models
{
    public class Room
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public int Space { get; set; }
        public int People { get; set; }
        public string? Description { get; set; }
        public string? HotelId { get; set; }
    }
}
