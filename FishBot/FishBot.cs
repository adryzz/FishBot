using Discord;
using Discord.WebSocket;

namespace FishBot;

public class FishBot : IAsyncDisposable
{
    protected DiscordSocketClient Client;
    public Configuration Config = new Configuration();
    protected CommandHandler? Handler = new CommandHandler();
    public FishBot()
    {
        if (File.Exists("config.json"))
        {
            Config = Configuration.FromFile("config.json");
        }
        else
        {
            Config = new Configuration();
            Config.Save("config.json");
        }

        Client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose
        });
        Client.Log += Client_Log;
        Client.Connected += Client_Connected;
        Client.Disconnected += Client_Disconnected;
        Client.MessageReceived += Client_MessageReceived;
    }

    private async Task Client_MessageReceived(SocketMessage arg)
    {
        //check for insults and ask for bug report
        if (arg.Content.Contains("fuck"))
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Red);
            embed.WithTitle("You seem angry...");
            embed.WithDescription("Would you like to [submit a bug report?](https://github.com/adryzz/FishBot/issues/new)");
            await arg.Channel.SendMessageAsync("", false, embed.Build());
        }
    }

    private async Task Client_Log(LogMessage arg)
    {
        
    }

    private async Task Client_Connected()
    {
        
    }
    
    private async Task Client_Disconnected(Exception arg)
    {
        
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