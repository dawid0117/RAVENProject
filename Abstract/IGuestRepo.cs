using BookingManagement___Projekt_zaliczeniowy.Models;

namespace BookingManagement___Projekt_zaliczeniowy.Abstract
{
    public interface IGuestRepo
    {
        bool AddGuest(Guest guest);
        bool RemoveGuest(string guestId);
        List<Guest> GetAllGuests();
        bool MakeReservation(Reservation reservation);
        Guest getGuestById(string guestId);
        Guest getByName(string identityName);
        bool IsReservedAtTime(string roomId,DateTime begin, DateTime end, Reservation resr = null);
        bool DeleteReservation(string id);
    }
}
