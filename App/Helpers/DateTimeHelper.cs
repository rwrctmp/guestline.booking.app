using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guestline.Booking.App.Helpers
{ 
    public static class DateTimeHelper
    {
        public static uint ToUIntDate(this DateTime date)
        {
            return (uint) (date.Year * 10000 + date.Month * 100 + date.Day);
        }

        public static IEnumerable<DateTime> GetFollowingDays(this DateTime startDay, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "The 'count' value must be non-negative.");
            }

            for (var i = 0; i < count; i++)
            {
                yield return startDay.AddDays(i);
            }
        }
    }
}
