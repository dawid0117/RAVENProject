namespace BookingManagement___Projekt_zaliczeniowy.Models
{
    public class Hotel
    {
        public string Id { get; set; }
        public string Street { get; set; }
        public string Name { get; set; }
        public float AvgMark { get; set; }
        public List<string> Rooms { get; set; } = new List<string>();
    }
}
