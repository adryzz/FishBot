using System.Diagnostics;
using Anilist4Net;
using Discord;
using Discord.WebSocket;
using FishBot.Logging;
using LogMessage = Discord.LogMessage;

namespace FishBot;

public class FishBot : IAsyncDisposable
{
    protected DiscordSocketClient Client;
    public Configuration Config = new Configuration();
    protected CommandHandler? Handler = new CommandHandler();

    public int Latency => Client.Latency;

    public Client AnimeClient = new Client();
    public FishBot()
    {
        if (File.Exists("config.json"))
        {
            Config = Configuration.FromFile("config.json");
        }
        else
        {
            Config.Save("config.json");
        }

        Client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose,
            GatewayIntents = GatewayIntents.None
        });
        Client.Log += Client_Log;
        Client.Connected += Client_Connected;
        Client.Disconnected += Client_Disconnected;
    }

    private async Task Client_Log(LogMessage arg)
    {
        await Program.Logger.LogAsync(new Logging.LogMessage(arg.Message, LogType.Runtime, (LogLevel)(int)arg.Severity));
    }

    private async Task Client_Connected()
    {
        await Program.Logger.LogAsync(new Logging.LogMessage("Connected", LogType.Network));
    }
    
    private async Task Client_Disconnected(Exception arg)
    {
        await Program.Logger.LogAsync(new Logging.LogMessage("Disconnected", LogType.Network));
    }

    public async Task LogInAsync()
    {
        if (Config.Token == "" || Config.Token == null) { throw new InvalidOperationException("The token isn't present in the configuration file."); }
        await Client.LoginAsync(TokenType.Bot, Config.Token);
        Handler = new CommandHandler();
        await Handler.InitialiseAsync(Client);
    }

    public async Task LogOutAsync()
    {
        if (Config.Token == "" ) { return; }
        await Client.LogoutAsync();
        Handler = null;
    }

    public async Task StartAsync()
    {
        await Client.StartAsync();
        await Client.SetStatusAsync(Config.Status);
        await Client.SetActivityAsync(new Game(Config.Game, ActivityType.Watching));
    }

    public async Task RefreshAsync()
    {
        await Client.SetStatusAsync(Config.Status);
        await Client.SetActivityAsync(new Game(Config.Game, ActivityType.Watching));
    }

    public async ValueTask DisposeAsync()
    {
        await LogOutAsync();
    }
}