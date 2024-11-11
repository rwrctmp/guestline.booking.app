using Guestline.Booking.App.Helpers;
using Guestline.Booking.App.Interfaces;
using Guestline.Booking.App.Models;
using Microsoft.Extensions.Configuration;

namespace Guestline.Booking.App.Services
{
    public class BookingRepository : IRepository
    {
        private readonly Dictionary<string, Hotel> _hotels;
        private readonly Dictionary<string, Models.Booking[]> _bookings;

        public BookingRepository(IConfiguration config)
        {
            var hotelsFile = config["hotels"] ?? throw new ArgumentException("JSON file paths cannot be null or empty.");
            var bookingFile = config["bookings"] ?? throw new ArgumentException("JSON file paths cannot be null or empty.");

            _hotels = JsonHelper.LoadCollectionFromFile<Hotel>(hotelsFile).ToDictionary(x => x.Id, x=> x);
            _bookings = JsonHelper.LoadCollectionFromFile<Models.Booking>(bookingFile).GroupBy(x => x.HotelId).ToDictionary(x => x.Key, x => x.ToArray());
        }

        public Hotel? GetHotelById(string id)
        {
            return _hotels.GetValueOrDefault(id);
        }

        public IEnumerable<Hotel> GetAllHotels()
        {
            return _hotels.Values;
        }

        public IEnumerable<Models.Booking> GetBookings(string hotelId, string roomType)
        {
            return _bookings.TryGetValue(hotelId, out var bookings)
                ? bookings.Where(x => x.RoomType == roomType)
                : Enumerable.Empty<Models.Booking>();
        }
    }
}
