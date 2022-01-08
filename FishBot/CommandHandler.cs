using Discord.WebSocket;
using System.Reflection;
using Discord.Interactions;
using RunMode = Discord.Interactions.RunMode;

namespace FishBot
{
    public class CommandHandler
    {
        private DiscordSocketClient? _client;
        private InteractionService? _service;

        public async Task InitialiseAsync(DiscordSocketClient socketClient)
        {
            _client = socketClient;
            _service = new InteractionService(_client, new InteractionServiceConfig { DefaultRunMode = RunMode.Async });
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            _client.InteractionCreated += HandleCommandAsync;
            _client.Ready += ClientOnReady;
        }

        private async Task ClientOnReady()
        {
            await _service.RegisterCommandsGloballyAsync();
        }

        private async Task HandleCommandAsync(SocketInteraction s)
        {
            var context = new SocketInteractionContext(_client, s);
            await _service.ExecuteCommandAsync(context, null);
        }
    }
}