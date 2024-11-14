using Guestline.Booking.App.Models;

namespace Guestline.Booking.App.Interfaces
{
    public interface IRepository
    {
        IReadOnlyCollection<Hotel> Hotels { get; }
        IReadOnlyCollection<Models.Booking> Bookings { get; }
    }
}
