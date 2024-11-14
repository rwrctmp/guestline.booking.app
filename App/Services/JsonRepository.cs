using Guestline.Booking.App.Helpers;
using Guestline.Booking.App.Interfaces;
using Guestline.Booking.App.Models;
using Microsoft.Extensions.Configuration;

namespace Guestline.Booking.App.Services
{
    public class JsonRepository : IRepository
    {
        public JsonRepository(IConfiguration config)
        {
            Hotels = JsonHelper.LoadCollectionFromFile<Hotel>(config["hotels"] ?? throw new Exception("Missing 'hotels' parameter."));
            Bookings = JsonHelper.LoadCollectionFromFile<Models.Booking>(config["bookings"] ?? throw new Exception("Missing 'bookings' parameter."));
        }

        public IReadOnlyCollection<Hotel> Hotels { get; private set; }

        public IReadOnlyCollection<Models.Booking> Bookings { get; private set; }

    }
}
