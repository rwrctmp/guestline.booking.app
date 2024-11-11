namespace Guestline.Booking.App.Exceptions
{
    public class InvalidCommandException(string commandName) : Exception($"Invalid command '{commandName}'");
}
