using System.Globalization;
using Guestline.Booking.App.Interfaces;
using Guestline.Booking.App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Guestline.Booking.UnitTest
{
    [TestFixture]
    internal class BookingServiceTest
    {
        private BookingService _service;

        [SetUp]
        public void Setup()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["hotels"]).Returns("hotels.json");
            mockConfig.Setup(c => c["bookings"]).Returns("bookings.json");
            var mockDateService = new Mock<IDateService>();
            mockDateService.Setup(x => x.GetCurrentDate())
                .Returns(DateTime.ParseExact("09/01/2024", "d", CultureInfo.InvariantCulture));

            var repository = new JsonRepository(mockConfig.Object, mockDateService.Object);
            
            var mockLogger = new Mock<ILogger<BookingService>>();
            _service = new BookingService(repository, mockDateService.Object, mockLogger.Object);
        }

        [TestCase("H1", 20240801, 20240830, "DBL", 2)]
        [TestCase("H1", 20240830, 20240830, "DBL", 2)]
        [TestCase("H1", 20240904, 20240904, "DBL", 2)]
        [TestCase("H1", 20240904, 20240930, "DBL", 2)]
        public void CheckAvailabilityOfEmptyRooms(string hotelId, int startDate, int endDate, string roomType, int expectedAvailability)
        {
            Assert.That(_service.CheckAvailability(hotelId, startDate, endDate, roomType), Is.EqualTo(expectedAvailability));
        }

        [TestCase("H1", 20240901, 20240901, "DBL", 1)]
        [TestCase("H1", 20240902, 20240902, "DBL", 1)]
        [TestCase("H1", 20240903, 20240903, "DBL", 1)]
        [TestCase("H1", 20240901, 20240902, "DBL", 1)]
        [TestCase("H1", 20240902, 20240903, "DBL", 1)]
        [TestCase("H1", 20240901, 20240903, "DBL", 1)]
        [TestCase("H1", 20240801, 20240901, "DBL", 1)]
        [TestCase("H1", 20240903, 20240905, "DBL", 1)]
        [TestCase("H1", 20240901, 20241001, "DBL", 1)]
        public void CheckAvailabilityOfPartlyBookedRooms(string hotelId, int startDate, int endDate, string roomType, int expectedAvailability)
        {
            Assert.That(_service.CheckAvailability(hotelId, startDate, endDate, roomType), Is.EqualTo(expectedAvailability));
        }

        [TestCase("H1", 20240902, 20240904, "SGL", 0)]
        [TestCase("H1", 20240901, 20240902, "SGL", 0)]
        public void CheckAvailabilityOfFullBookedRooms(string hotelId, int startDate, int endDate, string roomType, int expectedAvailability)
        {
            Assert.That(_service.CheckAvailability(hotelId, startDate, endDate, roomType), Is.EqualTo(expectedAvailability));
        }

        [TestCase("H1", 20240905, 20240905, "SGL", -1)]
        [TestCase("H1", 20240901, 20240905, "SGL", -1)]
        [TestCase("H1", 20240903, 20240908, "SGL", -2)]
        public void CheckAvailabilityOfOverbookedRooms(string hotelId, int startDate, int endDate, string roomType, int expectedAvailability)
        {
            Assert.That(_service.CheckAvailability(hotelId, startDate, endDate, roomType), Is.EqualTo(expectedAvailability));
        }

        [TestCase("H1", 0, "XXX")]
        [TestCase("H5", 1, "SGL")]
        [TestCase("H", 5, "1234")]
        public void SearchNoExistingHotelOrRoom(string hotelId, int days, string roomType)
        {
            Assert.That(_service.Search(hotelId, days, roomType), Is.Empty);
        }

        [Test]
        public void SearchOnlyOneDay()
        {
            var result = _service.Search("H1", 0, "DBL").ToArray();
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].StartDate, Is.EqualTo(20240901));
            Assert.That(result[0].EndDate, Is.EqualTo(20240901));
            Assert.That(result[0].RoomsCount, Is.EqualTo(1));
        }
        
        [Test]
        public void SearchMultipleDaysNoEmpty()
        {
            var result = _service.Search("H1", 6, "SGL").ToArray();
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].StartDate, Is.EqualTo(20240901));
            Assert.That(result[0].EndDate, Is.EqualTo(20240901));
            Assert.That(result[0].RoomsCount, Is.EqualTo(2));
            Assert.That(result[1].StartDate, Is.EqualTo(20240906));
            Assert.That(result[1].EndDate, Is.EqualTo(20240907));
            Assert.That(result[1].RoomsCount, Is.EqualTo(1));
        }
    }
}
