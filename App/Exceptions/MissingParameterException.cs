namespace Guestline.Booking.App.Exceptions
{
    internal class MissingParameterException(string parameterName) : InvalidConfigurationException($"Missing '{parameterName}' parameter.");
}
