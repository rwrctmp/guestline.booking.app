using Guestline.Booking.App.Commands;
using Guestline.Booking.App.Exceptions;
using Guestline.Booking.App.Interfaces;
using Moq;
using Guestline.Booking.App.Models;

namespace Guestline.Booking.UnitTest
{
    [TestFixture]
    internal class SearchCommandTest
    {
        private SearchCommand _command;

        [SetUp]
        public void Setup()
        {
            var mockService = new Mock<IBookingService>();
            _command = new SearchCommand(mockService.Object);
        }
        
        [Test]
        public void InvalidNumberOfParameters()
        {
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute(null));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute());
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "20"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "20", "SGT", "ExtraParam"));
        }

        [Test]
        public void InvalidNumberOfDays()
        {
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", null, "SGT"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "", "SGT"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "Invalid", "SGT"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "-1", "SGT"));
        }

        [Test]
        public void ParsingParameters()
        {
            var mockService = new Mock<IBookingService>();
            var command = new SearchCommand(mockService.Object);

            command.Execute("H1", "20", "SGL");
            mockService.Verify(x => x.Search("H1", 20, "SGL"), Times.Once);

            command.Execute("H2", "21", "DBL");
            mockService.Verify(x => x.Search("H2", 21, "DBL"), Times.Once);
        }

        [Test]
        public void CheckResults()
        {
            var mockService = new Mock<IBookingService>();
            mockService.Setup(x => x.Search("H1", 1, "SGL")).Returns(Enumerable.Empty<Availability>());
            mockService.Setup(x => x.Search("H1", 2, "SGL")).Returns(new []{new Availability(13112024, 13112024, 2)});
            mockService.Setup(x => x.Search("H1", 4, "SGL")).Returns(new []{new Availability(13112024, 13112024, 2), new Availability(14112024, 15112024, 1)});
            
            var command = new SearchCommand(mockService.Object);
            Assert.That(command.Execute("H1", "1", "SGL"), Is.EqualTo(""));
            Assert.That(command.Execute("H1", "2", "SGL"), Is.EqualTo("(13112024-13112024, 2)"));
            Assert.That(command.Execute("H1", "4", "SGL"), Is.EqualTo("(13112024-13112024, 2), (14112024-15112024, 1)"));
        }
    }
}
