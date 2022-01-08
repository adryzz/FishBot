using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using Discord.Interactions;

namespace FishBot
{
    public class CommandHandler
    {
        DiscordSocketClient Client;
        InteractionService Service;

        public async Task InitialiseAsync(DiscordSocketClient client)
        {
            Client = client;
            Service = new InteractionService(Client);
            await Service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Client.SlashCommandExecuted += HandleCommandAsync;
            await Service.RegisterCommandsGloballyAsync();
        }

        private async Task HandleCommandAsync(SocketSlashCommand s)
        {
            var context = new InteractionContext(Client, s, s.User, s.Channel);
            await Service.ExecuteCommandAsync(context, null);
        }
    }
}