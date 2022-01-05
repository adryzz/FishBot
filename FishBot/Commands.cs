using Discord.Commands;
using Discord;
using System.Diagnostics;


namespace FishBot
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Program.Bot.Config.Color.R, Program.Bot.Config.Color.G, Program.Bot.Config.Color.B);
            embed.WithTitle("AdryBot");
            embed.WithUrl("https://github.com/adryzz/fishbot/");
            embed.WithDescription($"Adryzz's bot\nDo **{Program.Bot.Config.CmdPrefix}commands** to see all the available commands!");
            embed.WithCurrentTimestamp();
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        
        
        [Command("ping", RunMode = RunMode.Async)]
        public async Task Ping()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int latency = Context.Client.Latency;
            long? ping = Utils.PingDNS();
            string text;
            watch.Stop();
            if (!ping.HasValue)
            {
                text = $"DNS: null ms\nGateway: {latency} ms\nTotal Evaluation: {watch.ElapsedMilliseconds} ms";
            }
            else
            {
                text = $"DNS: {ping} ms\nGateway: {latency} ms\nTotal Evaluation: {watch.ElapsedMilliseconds} ms";
            }
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Green);
            embed.WithTitle("Pong!");
            embed.WithDescription(text);
            embed.WithCurrentTimestamp();
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}