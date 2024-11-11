using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guestline.Booking.App.Interfaces;

namespace Guestline.Booking.App.Commands
{
    public class SearchCommand : ICommand
    {
        public string Name => "Search";
        public void Execute(params string[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
