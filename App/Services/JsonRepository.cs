using Guestline.Booking.App.Exceptions;
using Guestline.Booking.App.Helpers;
using Guestline.Booking.App.Interfaces;
using Guestline.Booking.App.Models;
using Microsoft.Extensions.Configuration;

namespace Guestline.Booking.App.Services
{
    public class JsonRepository : IRepository
    {
        public JsonRepository(IConfiguration config, IDateService dateService)
        {
            Hotels = LoadFromConfiguration<Hotel>(config, "hotels");
            Bookings = LoadFromConfiguration<Models.Booking>(config, "bookings");

            if (config.GetBoolValue("ignoreObsoleteData"))
            {
                var currentDate = dateService.GetCurrentDate().ToUIntDate(); 
                Bookings = Bookings.Where(x => x.Departure >= currentDate).ToArray();
            }
        }

        public IReadOnlyCollection<Hotel> Hotels { get; private set; }

        public IReadOnlyCollection<Models.Booking> Bookings { get; private set; }

        private IReadOnlyCollection<T> LoadFromConfiguration<T>(IConfiguration config, string parameterName)
        {
            var dataPath = config[parameterName] ?? throw new MissingParameterException(parameterName);

            try
            {
                var dataContent = File.ReadAllText(dataPath);
                return JsonHelper.LoadCollectionFromString<T>(dataContent);
            }
            catch (IOException ex)
            {
                throw new InvalidConfigurationException($"Cannot read database '{dataPath}'.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidConfigurationException($"Invalid database '{dataPath}'.", ex);
            }
        }

    }
}
