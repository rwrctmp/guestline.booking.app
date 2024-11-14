
namespace Guestline.Booking.App.Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        string Execute(params string[] parameters);
    }
}
