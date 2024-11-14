using Guestline.Booking.App.Commands;
using Guestline.Booking.App.Exceptions;
using Guestline.Booking.App.Interfaces;
using Moq;

namespace Guestline.Booking.UnitTest
{
    [TestFixture]
    internal class AvailabilityCommandTests
    {
        private AvailabilityCommand _command;

        [SetUp]
        public void Setup()
        {
            var mockService = new Mock<IBookingService>();
            _command = new AvailabilityCommand(mockService.Object);
        }
        
        [Test]
        public void InvalidNumberOfParameters()
        {
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute(null));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute());
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "20241111"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "20241111", "SGT", "ExtraParam"));
        }

        [Test]
        public void InvalidPeriod()
        {
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", null, "SGT"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "-", "SGT"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "InvalidPeriod", "SGT"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "Invalid-Period", "SGT"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "20241111-Invalid", "SGT"));
            Assert.Throws<InvalidCommandParametersException>(() => _command.Execute("H1", "Invalid-20241111", "SGT"));
        }

        [Test]
        public void ParsingParameters()
        {
            var mockService = new Mock<IBookingService>();
            var command = new AvailabilityCommand(mockService.Object);

            command.Execute("H1", "20241111", "SGL");
            mockService.Verify(x => x.CheckAvailability("H1", 20241111, 20241111, "SGL"), Times.Once);

            mockService.Invocations.Clear();
            command.Execute("H2", "20241111", "DBL");
            mockService.Verify(x => x.CheckAvailability("H2", 20241111, 20241111, "DBL"), Times.Once);

            mockService.Invocations.Clear();
            command.Execute("H3", "20220101-20220103", "DBL");
            mockService.Verify(x => x.CheckAvailability("H3", 20220101, 20220103, "DBL"), Times.Once);
        }

        [Test]
        public void CheckResults()
        {
            var mockService = new Mock<IBookingService>();
            mockService.Setup(x => x.CheckAvailability("H1", 20241111, 20241111, "SGL")).Returns(2);
            var command = new AvailabilityCommand(mockService.Object);

            Assert.That(command.Execute("H1", "20241111", "SGL"), Is.EqualTo("2"));
        }
    }
}
