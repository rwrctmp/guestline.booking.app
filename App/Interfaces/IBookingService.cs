using Guestline.Booking.App.Models;

namespace Guestline.Booking.App.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<Models.Booking> GetBookings(string hotelId, string roomType);
        int CheckAvailability(string hotelId, int startDate, int endDate, string roomTypeId);
        IEnumerable<Availability> Search(string hotelId, int days, string roomType);
    }
}
