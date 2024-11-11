using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guestline.Booking.App.Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        void Execute(params string[] parameters);
    }
}
