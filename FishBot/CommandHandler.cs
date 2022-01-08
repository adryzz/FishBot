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
            Client.SlashCommandExecuted += HandleCommandAsync;
            Client.Ready += ClientOnReady;
        }

        private async Task ClientOnReady()
        {
            await Service.RegisterCommandsToGuildAsync(528487200581615616);
        }

        private async Task HandleCommandAsync(SocketSlashCommand s)
        {
            var context = new InteractionContext(Client, s, s.User, s.Channel);
            await Service.ExecuteCommandAsync(context, null);
        }
    }
}