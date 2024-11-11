using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guestline.Booking.App.Models
{
    public class Booking
    {
        public string HotelId { get; set; }
        public uint Arrival { get; set; } 
        public uint Departure { get; set; }
        public string RoomType { get; set; }
        public string RoomRate { get; set; }
    }
}
