using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guestline.Booking.App.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Guestline.Booking.App.Helpers
{
    internal static class ConfigurationHelper
    {
        internal static bool GetBoolValue(this IConfiguration config, string key, bool defaultValue = default)
        {
            var value = config[key];
            if (value == null)
            {
                return defaultValue;
            }

            if (bool.TryParse(value, out var boolValue))
            {
                return boolValue;
            }

            throw new InvalidConfigurationException($"Invalid bool value for '{key}' parameter.");
        }
    }
}
