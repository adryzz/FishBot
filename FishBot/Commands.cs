using Discord.Commands;
using Discord;
using System.Diagnostics;
using Anilist4Net;
using Anilist4Net.Enums;
using FishBot.Logging;


namespace FishBot
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("help", RunMode = RunMode.Async)]
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
            long? ping = await Utils.PingDnsAsync();
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
                embed.Color = user.ProfileColour.ToColor();
                //embed.Description;
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "User not found.";
                embed.Color = Color.Red;
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                await e.Log();
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
                    await Context.Channel.SendMessageAsync(result.SiteUrl);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Anime not found.";
                    embed.Color = Color.Red;
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Anime not found.";
                embed.Color = Color.Red;
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                await e.Log();
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
                    await Context.Channel.SendMessageAsync(result.SiteUrl);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Manga not found.";
                    embed.Color = Color.Red;
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Manga not found.";
                embed.Color = Color.Red;
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                await e.Log();
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
                    await Context.Channel.SendMessageAsync(result.SiteUrl);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Media not found.";
                    embed.Color = Color.Red;
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Media not found.";
                embed.Color = Color.Red;
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                await e.Log();
            }
        }
        
        [Command("animeinfo", RunMode = RunMode.Async)]
        public async Task AnimeInfo([Remainder] string name)
        {
            try
            {
                var result = await Program.Bot.AnimeClient.GetMediaBySearch(name, MediaTypes.ANIME);

                if (result != null)
                {
                    var embed = new EmbedBuilder();
                    embed.Url = result.SiteUrl;
                    embed.Title = result.RomajiTitle;
                    embed.Description = Utils.FormatMarkdown(result.DescriptionMd);
                    embed.AddField(new EmbedFieldBuilder { Name = ":star: Rating", Value = $"{result.AverageScore}/100", IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":book: Genres", Value = string.Join(", ", result.Genres), IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":clapper: Episodes", Value = result.Episodes });
                    embed.AddField(new EmbedFieldBuilder { Name = ":clock10: Duration", Value = $"{result.Duration}min/episode", IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":heart: Favourites", Value = result.Favourites, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":popcorn: Popularity", Value = result.Popularity, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":eggplant: Adult", Value = result.IsAdult, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":family: Relations", Value = Utils.FormatMediaRelations(result.MediaRelations), IsInline = true });
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Anime not found.";
                    embed.Color = Color.Red;
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Anime not found.";
                embed.Color = Color.Red;
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                await e.Log();
            }
        }
        
        [Command("mangainfo", RunMode = RunMode.Async)]
        public async Task MangaInfo([Remainder] string name)
        {
            try
            {
                var result = await Program.Bot.AnimeClient.GetMediaBySearch(name, MediaTypes.MANGA);

                if (result != null)
                {
                    var embed = new EmbedBuilder();
                    embed.Url = result.SiteUrl;
                    embed.Title = result.RomajiTitle;
                    embed.Description = Utils.FormatMarkdown(result.DescriptionMd);
                    embed.AddField(new EmbedFieldBuilder { Name = ":star: Rating", Value = $"{result.AverageScore}/100", IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":book: Genres", Value = string.Join(", ", result.Genres), IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":books: Volumes", Value = result.Volumes, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":bookmark: Chapters", Value = result.Chapters, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":heart: Favourites", Value = result.Favourites, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":popcorn: Popularity", Value = result.Popularity, IsInline = true });
                    embed.AddField(new EmbedFieldBuilder { Name = ":eggplant: Adult", Value = result.IsAdult, IsInline = true });
                    //embed.AddField(new EmbedFieldBuilder { Name = ":family: Relations", Value = Utils.FormatMediaRelations(result.MediaRelations), IsInline = true});
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Manga not found.";
                    embed.Color = Color.Red;
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Manga not found.";
                embed.Color = Color.Red;
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                await e.Log();
            }
        }
    }
}