using Guestline.Booking.App.Exceptions;
using Guestline.Booking.App.Interfaces;

namespace Guestline.Booking.App.Commands
{
    public class SearchCommand : ICommand
    {
        private readonly IBookingService _service;

        public SearchCommand(IBookingService service)
        {
            _service = service;
        }

        public string Name => "Search";
        public string Execute(params string[] parameters)
        {
            if (parameters == null || parameters.Length != 3)
            {
                throw new InvalidCommandParametersException(this.Name, $"Invalid number of parameters. Expected 3 parameters, but received {parameters?.Length}.");
            }

            var hotelId = parameters[0];

            if (!int.TryParse(parameters[1], out var days) || days < 0)
            {
                throw new InvalidCommandParametersException(this.Name, $"Invalid value for numbers of day. The second parameter must be a non-negative integer, but received: '{parameters[1]}'.");
            }

            var roomType = parameters[2];

            return string.Join(", ", _service.Search(hotelId, days, roomType).Select(x=> $"({x.StartDate}-{x.EndDate}, {x.RoomsCount})"));
        }
    }
}
