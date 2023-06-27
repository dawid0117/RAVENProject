using BookingManagement___Projekt_zaliczeniowy.Abstract;
using BookingManagement___Projekt_zaliczeniowy.Models;

namespace BookingManagement___Projekt_zaliczeniowy.Concrete
{
    public class RoomRepo : IRoomRepo
    {
        public bool AddRoom(Room room)
        {
            using(var session = Session.Store.OpenSession())
            {
                if(room != null)
                {
                    session.Store(room);
                    session.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<Room> GetAllRooms()
        {
            using (var session = Session.Store.OpenSession())
            {
                var rooms = session.Query<Room>().ToList();
                return rooms;
            }
        }

        public Room GetRoomById(string roomId)
        {
            using (var session = Session.Store.OpenSession())
            {
                var room = session.Query<Room>().Where(x => x.Id == roomId).First();
                return room;
            }
        }

        public Room GetRoomByName(string roomName)
        {
            using (var session = Session.Store.OpenSession())
            {
                var room = session.Query<Room>().Where(x => x.Name == roomName).First();
                return room;
            }
        }

        public bool RemoveRoom(string roomId)
        {
            using (var session = Session.Store.OpenSession())
            {
                var room = session.Query<Room>().Where(x => x.Id == roomId).First();
                if (room != null)
                {
                    session.Delete(room);
                    session.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
