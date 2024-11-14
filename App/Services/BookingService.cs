using Guestline.Booking.App.Helpers;
using Guestline.Booking.App.Interfaces;
using Guestline.Booking.App.Models;
using Microsoft.Extensions.Logging;

namespace Guestline.Booking.App.Services
{
    public class BookingService : IBookingService
    {
        private record struct CacheKey(string HotelId, string RoomTypeId);
        private class CacheData
        {
            public Room[] Rooms { get; set; }
            public Models.Booking[] Bookings { get; set; }

            public CacheData(Room[] rooms, params Models.Booking[] bookings)
            {
                Rooms = rooms;
                Bookings = bookings;
            }
        }

        private readonly Dictionary<CacheKey, CacheData> _cache = new ();
        private readonly IDateService _dateService;

        public BookingService(IRepository repository, IDateService dateService, ILogger<BookingService> logger)
        {
            _dateService = dateService;

            foreach (var hotel in repository.Hotels)
            {
                foreach (var roomType in hotel.Rooms.GroupBy(x => x.RoomType))
                {
                    _cache.Add(new CacheKey(hotel.Id, roomType.Key), new CacheData(roomType.ToArray()));
                }
            }

            var currentDate = _dateService.GetCurrentDate().ToUIntDate();    

            foreach (var booking in repository.Bookings.GroupBy(x => new CacheKey(x.HotelId, x.RoomType)))
            {
                if (!_cache.TryGetValue(booking.Key, out var data))
                {
                    logger.LogWarning($"Booking ignored for not existing room ({booking.Key.HotelId}, {booking.Key.RoomTypeId})");
                    continue;
                }

                data.Bookings = booking.Where(x => x.Departure >= currentDate).ToArray();
            }
        }

        public IEnumerable<Models.Booking> GetBookings(string hotelId, string roomTypeId)
        {
            if (_cache.TryGetValue(new CacheKey(hotelId, roomTypeId), out var data))
            {
                return data.Bookings;
            }
            
            return Enumerable.Empty<Models.Booking>();
        }

        public int CheckAvailability(string hotelId, int startDate, int endDate, string roomTypeId)
        {
            ThrowIfRoomIsInvalid(hotelId, roomTypeId);

            if (!_cache.TryGetValue(new CacheKey(hotelId, roomTypeId), out var data))
            {
                return 0;
            }

            var count = data.Rooms.Length;

            if (count == 0)
            {
                return 0;
            }

            var booked = data.Bookings.Count(x => x.Arrival <= endDate && x.Departure >= startDate);

            return count - booked;
        }

        public IEnumerable<Availability> Search(string hotelId, int days, string roomTypeId)
        {
            ThrowIfRoomIsInvalid(hotelId, roomTypeId);

            if (days < 0 || !_cache.TryGetValue(new CacheKey(hotelId, roomTypeId), out var data))
            {
                yield break;
            }

            var dayNumbers = _dateService.GetCurrentDate().GetFollowingDays(days + 1)
                .Select(x => x.ToUIntDate()).ToArray();

            var firstDay = dayNumbers[0];
            var lastDay = dayNumbers[days];

            var availability = new int[days + 1];
            Array.Fill(availability, data.Rooms.Length);

            foreach (var booking in data.Bookings)
            {
                if (booking.Arrival > lastDay || booking.Departure < firstDay)
                {
                    continue;
                }

                var startDate = Math.Max(booking.Arrival, firstDay);
                var startIdx = Math.Abs(Array.BinarySearch<uint>(dayNumbers, startDate));

                for (var i = startIdx; i <= days  && dayNumbers[i] <= booking.Departure; i++)
                {
                    availability[i]--;
                }
            }

            var lastAvailability = 0;
            var startSequenceIdx = 0;
            for (var i = 0; i <= days; i++)
            {
                var currentAvailability = Math.Max(availability[i], 0);

                if (lastAvailability == currentAvailability)
                {
                    continue;
                }

                if (lastAvailability != 0)
                {
                    yield return new Availability(dayNumbers[startSequenceIdx], dayNumbers[i - 1], (uint)lastAvailability);
                }

                startSequenceIdx = i;
                lastAvailability = currentAvailability;
            }

            if (lastAvailability != 0)
            {
                yield return new Availability(dayNumbers[startSequenceIdx], dayNumbers[days], (uint)lastAvailability);
            }
        }

        private void ThrowIfRoomIsInvalid(string hotelId, string roomType)
        {
            if (hotelId == null)
            {
                throw new ArgumentNullException(nameof(hotelId));
            }

            if (hotelId == string.Empty)
            {
                throw new ArgumentException("Argument cannot be empty.", nameof(hotelId));
            }

            if (roomType == null)
            {
                throw new ArgumentNullException(nameof(roomType));
            }

            if (roomType == string.Empty)
            {
                throw new ArgumentException("Argument cannot be empty.", nameof(roomType));
            }
        }
    }
}
