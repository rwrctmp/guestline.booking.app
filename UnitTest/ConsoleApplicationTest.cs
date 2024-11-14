using Guestline.Booking.App.Interfaces;
using Moq;
using Guestline.Booking.App;
using Guestline.Booking.App.Exceptions;
using Microsoft.Extensions.Logging;

namespace Guestline.Booking.UnitTest
{
    public class ConsoleApplicationTest
    {
        private Mock<ICommand> _commandMock, _commandMock2;
        private ConsoleApplication _consoleApplication;

        [SetUp]
        public void Setup()
        {
            _commandMock = new Mock<ICommand>();
            _commandMock.Setup(c => c.Name).Returns("Command");
            _commandMock2 = new Mock<ICommand>();
            _commandMock2.Setup(c => c.Name).Returns("Command2");
            var loggerMock = new Mock<ILogger<ConsoleApplication>>();
            _consoleApplication = new ConsoleApplication(new[] { _commandMock.Object, _commandMock2.Object }, loggerMock.Object);
        }

        [Test]
        public void ParseCommandWithoutParameters()
        {
            PrepareMock();

            _consoleApplication.ExecuteCommand("Command()");
            _consoleApplication.ExecuteCommand("Command2()");
            
            _commandMock.Verify(c => c.Execute(), Times.Once);
            _commandMock2.Verify(c => c.Execute(), Times.Once);
        }

        [Test]
        public void ParseCommandWithMultipleParameters()
        {
            PrepareMock();

            _consoleApplication.ExecuteCommand("Command()");
            _consoleApplication.ExecuteCommand("Command(1)");
            _consoleApplication.ExecuteCommand("Command(1,2)");
            _consoleApplication.ExecuteCommand("Command(1,2,3)");
            _consoleApplication.ExecuteCommand("Command(1,2,3,4)");

            _commandMock.Verify(c => c.Execute(), Times.Once);
            _commandMock.Verify(c => c.Execute("1"), Times.Once);
            _commandMock.Verify(c => c.Execute("1", "2"), Times.Once);
            _commandMock.Verify(c => c.Execute("1", "2", "3"), Times.Once);
            _commandMock.Verify(c => c.Execute("1", "2", "3", "4"), Times.Once);
        }

        [Test]
        public void InvalidCommandName()
        {
            PrepareMock();
            Assert.Catch<InvalidCommandException>(()=>_consoleApplication.ExecuteCommand("Test()"));
        }

        private void PrepareMock()
        {
            _commandMock.Invocations.Clear();
        }
    }
}
