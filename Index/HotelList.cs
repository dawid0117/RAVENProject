using BookingManagement___Projekt_zaliczeniowy.Abstract;
using BookingManagement___Projekt_zaliczeniowy.Models;
using Raven.Client.Documents.Indexes;
using System.Security.Policy;

namespace BookingManagement___Projekt_zaliczeniowy.Index
{
    public class HotelList : AbstractIndexCreationTask<Hotel, HotelList.IndexEntry>
    {

        public class IndexEntry
        {
           public string Id { get; set; }
           public string Name { get; set; }
           public string Street { get; set; }
           public float AvgMark { get; set; }   
           public int Rooms { get; set; }

        }

        public HotelList()
        {
            Map = hotels => from hotel in hotels
                            select new IndexEntry
                            {
                                Name = hotel.Name,
                                Street = hotel.Street,
                                AvgMark = hotel.AvgMark,
                                Rooms = hotel.Rooms.Count    
                            };
        }
    }
}
