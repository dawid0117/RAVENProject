using BookingManagement___Projekt_zaliczeniowy.Index;
using BookingManagement___Projekt_zaliczeniowy.Models;

namespace BookingManagement___Projekt_zaliczeniowy.Abstract
{
    public interface IHotelRepo
    {
        bool AddHotel(Hotel hotel);
        bool AddRoom(string roomId, string hotelId);
        bool RemoveHotel(string hotelId);
        List<Room> GetAllRooms(string hotelId);
        List<Hotel> GetAllHotels();
        Hotel GetById(string id);
    }
}
