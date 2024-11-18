using Guestline.Booking.App.Interfaces;
using Guestline.Booking.App.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Globalization;

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
            var mockDateService = new Mock<IDateService>();
            _repository = new JsonRepository(mockConfig.Object, mockDateService.Object);
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
        public void GetBookingsFiltered()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["hotels"]).Returns("hotels.json");
            mockConfig.Setup(c => c["bookings"]).Returns("bookings.json");
            mockConfig.Setup(c => c["ignoreObsoleteData"]).Returns("true");
            var mockDateService = new Mock<IDateService>();
            mockDateService.Setup(x => x.GetCurrentDate())
                .Returns(DateTime.ParseExact("09/05/2024", "d", CultureInfo.InvariantCulture));
            var repository = new JsonRepository(mockConfig.Object, mockDateService.Object);

            Assert.That(repository.Bookings.Count, Is.EqualTo(4));

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