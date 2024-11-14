using Guestline.Booking.App.Exceptions;
using Guestline.Booking.App.Interfaces;

namespace Guestline.Booking.App.Commands
{
    public class AvailabilityCommand : ICommand
    {
        private readonly IBookingService _service;

        public AvailabilityCommand(IBookingService service)
        {
            _service = service;
        }

        public string Name => "Availability";
        public string Execute(params string[] parameters)
        {
            if (parameters == null || parameters.Length != 3)
            {
                throw new InvalidCommandParametersException(this.Name, $"Invalid number of parameters. Expected 3 parameters, but received {parameters?.Length}.");
            }

            var hotelId = parameters[0];

            if (!TryParsePeriod(parameters[1], out var startDate, out var endDate))
            {
                throw new InvalidCommandParametersException(this.Name,  $"Invalid value for data range. The second parameter must be a in format yyyyMMdd-yyyyMMdd or yyyyMMdd, but received: '{parameters[1]}'.");
            }

            var roomType = parameters[2];

            return _service.CheckAvailability(hotelId, startDate, endDate, roomType).ToString();
        }

        private bool TryParsePeriod(string input, out int startDate, out int endDate)
        {
            startDate = endDate = 0;
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            
            var separatorIdx = input.IndexOf('-');

            if (separatorIdx < 0)
            {
                if (int.TryParse(input, out startDate))
                {
                    endDate = startDate;
                    return true;
                }

                return false;
            }

            return int.TryParse(input[..separatorIdx], out startDate) && 
                   int.TryParse(input[(separatorIdx + 1)..], out endDate);
        }
    }
}
