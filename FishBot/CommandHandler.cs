using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using Discord.Interactions;
using RunMode = Discord.Interactions.RunMode;

namespace FishBot
{
    public class CommandHandler
    {
        DiscordSocketClient Client;
        InteractionService Service;

        public async Task InitialiseAsync(DiscordSocketClient client)
        {
            Client = client;
            Service = new InteractionService(Client, new InteractionServiceConfig { DefaultRunMode = RunMode.Async });
            await Service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Client.InteractionCreated += HandleCommandAsync;
            Client.Ready += ClientOnReady;
        }

        private async Task ClientOnReady()
        {
            await Service.RegisterCommandsToGuildAsync(528487200581615616);
        }

        private async Task HandleCommandAsync(SocketInteraction s)
        {
            var context = new SocketInteractionContext(Client, s);
            await s.RespondAsync("a");
            await Service.ExecuteCommandAsync(context, null);
        }
    }
}