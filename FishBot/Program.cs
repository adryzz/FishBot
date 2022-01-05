
namespace FishBot
{
    static class Program
    {
        public static FishBot Bot = new FishBot();

        public static dynamic Logger;
        
        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            await Bot.LogInAsync();
            await Bot.StartAsync();
            
            while (true)
            {
                Console.ReadKey();
            }
        }

        private static void ConsoleOnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            Bot.DisposeAsync();
        }
    }
}