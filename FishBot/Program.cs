
using FishBot.Logging;

namespace FishBot
{
    static class Program
    {
        public static FishBot Bot = new FishBot();

        public static Logger Logger = new Logger();
        
        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            await Bot.LogInAsync();
            await Bot.StartAsync();

            await Task.Delay(-1);
        }

        private static async void ConsoleOnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            await Bot.DisposeAsync();
        }
    }
}