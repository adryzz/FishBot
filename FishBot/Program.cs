
namespace FishBot
{
    static class Program
    {
        public static FishBot Bot = new FishBot();

        public static dynamic Logger;
        
        static async Task Main(string[] args)
        {
            await Bot.LogInAsync();
            await Bot.StartAsync();
        }
    }
}