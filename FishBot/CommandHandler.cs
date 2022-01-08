using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using Discord;
using Discord.Interactions;

namespace FishBot
{
    public class CommandHandler
    {
        DiscordSocketClient Client;
        CommandService Service;

        public async Task InitialiseAsync(DiscordSocketClient client)
        {
            Client = client;
            Service = new CommandService();
            await Service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Client.SlashCommandExecuted += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketSlashCommand command)
        {
            var context = new 
            int argPos = 0;
            if (msg.HasStringPrefix(Program.Bot.Config.CmdPrefix, ref argPos) || msg.HasMentionPrefix(Client.CurrentUser, ref argPos))
            {
                var result = await Service.ExecuteAsync(context, null);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    //log
                }
            }
        }
    }
}