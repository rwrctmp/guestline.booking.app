﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Guestline.Booking.App.Models
{
    public class Hotel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
