namespace Guestline.Booking.App.Exceptions
{
    public class InvalidCommandParametersException(string commandName, string message) : Exception($"Invalid parameters for command '{commandName}'. {message}");
}
