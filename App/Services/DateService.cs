using Guestline.Booking.App.Interfaces;
using System.Globalization;
using System;

namespace Guestline.Booking.App.Services
{
    public class DateService : IDateService
    {
        public DateTime GetCurrentDate()
        {
            return DateTime.Today;
        }
    }
}
