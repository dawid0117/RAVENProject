using BookingManagement___Projekt_zaliczeniowy.Abstract;
using BookingManagement___Projekt_zaliczeniowy.Index;
using BookingManagement___Projekt_zaliczeniowy.Models;

namespace BookingManagement___Projekt_zaliczeniowy.Concrete
{
    public class HotelRepo : IHotelRepo
    {
        public bool AddHotel(Hotel hotel)
        {
            using(var session = Session.Store.OpenSession())
            {
                if(hotel != null)
                {
                    session.Store(hotel);
                    session.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<Hotel> GetAllHotels()
        {
            using (var session = Session.Store.OpenSession())
            {
                var hotels = session.Query<Hotel>().ToList();
                return hotels;
            }
        }

        public List<Room> GetAllRooms(string hotelId)
        {
            using (var session = Session.Store.OpenSession())
            {
                List<Room> roomsList = new List<Room>();
                var hotel = session.Query<Hotel>().Where(x => x.Id == hotelId).First();
                foreach(var room in hotel.Rooms)
                {
                    roomsList.Add(session.Query<Room>().Where(x => x.Id == room).First());
                }
                return roomsList;
            }
        }

        public bool RemoveHotel(string hotelId)
        {
            using (var session = Session.Store.OpenSession())
            {
                var hotel = session.Query<Hotel>().Where(x => x.Id == hotelId).First();
                if (hotel != null)
                {
                    session.Delete(hotel);
                    session.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public Hotel GetById(string id)
        {
            using (var session = Session.Store.OpenSession())
            {
                var hotel = session.Query<Hotel>().Where(x => x.Id == id).First();
                if (hotel != null)
                    return hotel;
                return null;
            }
        }

        public bool AddRoom(string roomId, string hotelId)
        {
            using (var session = Session.Store.OpenSession())
            {
                var hotel = session.Query<Hotel>().Where(x => x.Id == hotelId).First();
                if (hotel != null)
                {
                    if (hotel.Rooms != null)
                        hotel.Rooms.Add(roomId);
                    else
                    {
                        hotel.Rooms = new List<string> { roomId };
                    }
                    session.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
