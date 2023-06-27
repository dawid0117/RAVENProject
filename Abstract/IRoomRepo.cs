using BookingManagement___Projekt_zaliczeniowy.Models;

namespace BookingManagement___Projekt_zaliczeniowy.Abstract
{
    public interface IRoomRepo
    {
        bool AddRoom(Room room);
        bool RemoveRoom(string roomId);
        List<Room> GetAllRooms();
        Room GetRoomById(string roomId);
        Room GetRoomByName(string roomName);

    }
}
