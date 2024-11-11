using Guestline.Booking.App.Models;

namespace Guestline.Booking.App.Interfaces
{
    public interface IRepository
    {
        Hotel GetHotelById(string id);
        IEnumerable<Hotel> GetAllHotels();
        IEnumerable<Models.Booking> GetBookings(string hotelId, string roomType);
    }
}
