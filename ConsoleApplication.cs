using Guestline.Booking.App.Exceptions;
using Guestline.Booking.App.Interfaces;

namespace Guestline.Booking.App
{
    public class ConsoleApplication
    {
        private readonly Dictionary<string, ICommand> _commands;

        public ConsoleApplication(IEnumerable<ICommand> commands)
        {
            _commands = commands.ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
        }

        public void Run()
        {
            string? line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                ExecuteCommand(line);
            }
        }

        public void ExecuteCommand(string line)
        {
            var openIdx = line.IndexOf('(');

            if (openIdx <= 0)
            {
                throw new InvalidDataException();
            }

            var closeIdx = line.IndexOf(')');

            if (openIdx > closeIdx)
            {
                throw new InvalidDataException();
            }

            var commandName = line[..openIdx].Trim();

            if (!_commands.TryGetValue(commandName, out var command))
            {
                throw new InvalidCommandException(commandName);
            }

            var commandParameters = ParseParameters(line.AsSpan().Slice(openIdx + 1, closeIdx - openIdx - 1));

            if (commandParameters.Length == 1 && commandParameters[0].Length == 0)
            {
                command.Execute();
                return;
            }

            command.Execute(commandParameters);
        }

        private string[] ParseParameters(ReadOnlySpan<char> paramText)
        {
            Span<Range> rangesOfParams = stackalloc Range[16];
            var numberOfParams = paramText.Split(rangesOfParams, ',');
            var paramsArray = new string[numberOfParams];

            for (var i = 0; i < numberOfParams; i++)
            {
                paramsArray[i] = paramText[rangesOfParams[i]].Trim().ToString();
            }

            return paramsArray;
        }
    }
}
