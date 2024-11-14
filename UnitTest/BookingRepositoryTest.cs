using Guestline.Booking.App.Interfaces;
using Guestline.Booking.App.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Guestline.Booking.UnitTest
{
    [TestFixture]
    public class BookingRepositoryTests
    {
        private IRepository _repository;

        [SetUp]
        public void Setup()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["hotels"]).Returns("hotels.json");
            mockConfig.Setup(c => c["bookings"]).Returns("bookings.json");
            _repository = new JsonRepository(mockConfig.Object);
        }

        [Test]
        public void GetBookings()
        {
            var bookings = _repository.Bookings;
            Assert.NotNull(bookings);
            Assert.That(bookings.Count, Is.EqualTo(5));

            var firstRecord = bookings.First();
            Assert.That(firstRecord.HotelId, Is.EqualTo("H1"));
            Assert.That(firstRecord.RoomType, Is.EqualTo("DBL"));
            Assert.That(firstRecord.Arrival, Is.EqualTo(20240901));
            Assert.That(firstRecord.Departure, Is.EqualTo(20240903));
        }

        [Test]
        public void GetHotels()
        {
            var hotels = _repository.Hotels;
            Assert.That(hotels, Is.Not.Null);
            Assert.That(hotels.Count, Is.EqualTo(1));

            var hotel = hotels.First();
            Assert.That(hotel.Id, Is.EqualTo("H1"));
            Assert.That(hotel.Name, Is.EqualTo("Hotel California"));
            Assert.That(hotel.Rooms, Is.Not.Null);
            Assert.That(hotel.Rooms.Count, Is.EqualTo(4));
        }
    }
}