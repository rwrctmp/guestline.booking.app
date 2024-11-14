namespace Guestline.Booking.App.Models
{
    public class Booking
    {
        public string HotelId { get; set; }
        public uint Arrival { get; set; } 
        public uint Departure { get; set; }
        public string RoomType { get; set; }
    }
}
