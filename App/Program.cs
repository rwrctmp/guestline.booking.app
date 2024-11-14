using Guestline.Booking.App;
using Guestline.Booking.App.Commands;
using Guestline.Booking.App.Interfaces;
using Guestline.Booking.App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

IConfiguration config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
            .AddConsole();
    })
    .AddSingleton(config)
    .AddSingleton<IDateService, DateService>()
    .AddSingleton<IRepository, JsonRepository>()
    .AddSingleton<IBookingService, BookingService>()
    .AddSingleton<ConsoleApplication>()
    .AddSingleton<ICommand, SearchCommand>()
    .AddSingleton<ICommand, AvailabilityCommand>()
    .BuildServiceProvider();

var app = serviceProvider.GetRequiredService<ConsoleApplication>();
app.Run();
