using BookingManagement___Projekt_zaliczeniowy.Models;
using Raven.Client.Documents.Indexes;

namespace BookingManagement___Projekt_zaliczeniowy.Index
{
    public class ReservationByTime : AbstractIndexCreationTask<Reservation, ReservationByTime.IndexEntry> 
    {
        public class IndexEntry
        {
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
            public string roomId { get; set; }
        }

        public ReservationByTime()
        {
            Map = reservations => from reservation in reservations
                                 select new IndexEntry
                                 {
                                     roomId= reservation.RoomId,
                                     Begin = reservation.begin,
                                     End = reservation.end
                                 };
        }
    }
}
