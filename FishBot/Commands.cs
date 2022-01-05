using Discord.Commands;
using Discord;
using System.Diagnostics;
using Anilist4Net;
using Anilist4Net.Enums;


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

        [Command("user", RunMode = RunMode.Async)]
        public async Task User(string? userName = null)
        {
            try
            {
                User user = await Program.Bot.AnimeClient.GetUserByName(userName ?? Context.User.Username);
                var embed = new EmbedBuilder();
                embed.Author = new EmbedAuthorBuilder {Name = user.Name, Url = user.SiteUrl, IconUrl = user.MediumAvatar };
                embed.Color = Utils.ConvertUserProfileColor(user.ProfileColour);
                //embed.Description;
                Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "User not found.";
                embed.Color = Color.Red;
                Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
        
        [Command("anime", RunMode = RunMode.Async)]
        public async Task Anime([Remainder] string name)
        {
            try
            {
                var result = await Program.Bot.AnimeClient.GetMediaBySearch(name, MediaTypes.ANIME);

                if (result != null)
                {
                    Context.Channel.SendMessageAsync(result.SiteUrl);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Anime not found.";
                    embed.Color = Color.Red;
                    Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Anime not found.";
                embed.Color = Color.Red;
                Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
        
        [Command("manga", RunMode = RunMode.Async)]
        public async Task Manga([Remainder] string name)
        {
            try
            {
                var result = await Program.Bot.AnimeClient.GetMediaBySearch(name, MediaTypes.MANGA);

                if (result != null)
                {
                    Context.Channel.SendMessageAsync(result.SiteUrl);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Manga not found.";
                    embed.Color = Color.Red;
                    Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Manga not found.";
                embed.Color = Color.Red;
                Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
        
        [Command("media", RunMode = RunMode.Async)]
        public async Task Media([Remainder] string name)
        {
            try
            {
                var result = await Program.Bot.AnimeClient.GetMediaBySearch(name);

                if (result != null)
                {
                    Context.Channel.SendMessageAsync(result.SiteUrl);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Media not found.";
                    embed.Color = Color.Red;
                    Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Media not found.";
                embed.Color = Color.Red;
                Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
    }
}