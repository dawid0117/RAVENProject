using BookingManagement___Projekt_zaliczeniowy.Abstract;
using BookingManagement___Projekt_zaliczeniowy.Index;
using BookingManagement___Projekt_zaliczeniowy.Models;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents.Indexes;

namespace BookingManagement___Projekt_zaliczeniowy.Concrete
{
    public class GuestRepo : IGuestRepo
    {
        public bool AddGuest(Guest guest)
        {
            using(var session = Session.Store.OpenSession())
            {
                if (guest != null)
                {

                    session.Store(guest);
                    session.SaveChanges();
                    return true;
                }
                return false;
            }

        }

        public List<Guest> GetAllGuests()
        {
            using (var session = Session.Store.OpenSession())
            {
                var guests = session.Query<Guest>().ToList(); 
                return guests;
            }
        }

        public Guest getGuestById(string guestId)
        {
            using (var session = Session.Store.OpenSession())
            {
                var guest = session.Load<Guest>(guestId);
                return guest;
            }
        }

        public Guest getByName(string identityName)
        {
            using (var session = Session.Store.OpenSession())
            {
                if(identityName != null)
                {
                    var guest = session.Query<Guest>().Where(x => x.Email == identityName).FirstOrDefault();
                    if(guest != null)
                    {
                        return guest;
                    }
                }
                return null;
            }
        }

        public bool MakeReservation(Reservation reservation)
        {
            using (var session = Session.Store.OpenSession())
            {
                try
                {
                    session.Store(reservation);
                    session.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }             
            }
        }

        public bool RemoveGuest(string guestId)
        {
            using (var session = Session.Store.OpenSession())
            {
                var guest = session.Load<Guest>(guestId);
                if (guest != null)
                {
                    session.Delete(guest);
                    session.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool IsReservedAtTime(string roomId,DateTime begin, DateTime end,Reservation resr = null)
        {
            
            using (var session = Session.Store.OpenSession())
            {
                var reservation = session.Query<ReservationByTime.IndexEntry,ReservationByTime>()
                    .Where(x => x.roomId == roomId).ToList();

                bool contains = false;
                foreach(var reservationEntry in reservation)
                {
                    if (resr != null)
                    {
                        if (((begin >= reservationEntry.Begin && begin < reservationEntry.End &&
                            (end <= reservationEntry.End || end >= reservationEntry.End) && end > begin) ||
                            (begin <= reservationEntry.Begin && (end > begin && (end <= reservationEntry.End
                            || end >= reservationEntry.End))) || (begin >= reservationEntry.Begin
                            && end <= reservationEntry.End)) && begin != resr.begin && end != resr.end)
                        {
                            contains = true;
                        }
                        else if (begin > reservationEntry.End && end > begin)
                            break;
                        if (contains) break;
                    }
                    else
                    {
                        if ((begin >= reservationEntry.Begin && begin < reservationEntry.End &&
                            (end <= reservationEntry.End || end >= reservationEntry.End) && end > begin) ||
                            (begin <= reservationEntry.Begin && (end > begin && (end <= reservationEntry.End
                            || end >= reservationEntry.End))) || (begin >= reservationEntry.Begin
                            && end <= reservationEntry.End))
                        {
                            contains = true;
                        }
                        else if (begin > reservationEntry.End && end > begin)
                            break;
                        if (contains) break;
                    }
                }

                return contains;

            }
        }

        public bool DeleteReservation(string id)
        {
            using(var session = Session.Store.OpenSession())
            {
                try
                {
                    var reservation = session.Load<Reservation>(id);
                    session.Delete(reservation);
                    session.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

    }
}
