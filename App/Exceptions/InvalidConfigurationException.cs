using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guestline.Booking.App.Exceptions
{
    public class InvalidConfigurationException : ApplicationException
    {
        public InvalidConfigurationException(string message) : base(message)
        {
        }

        public InvalidConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
