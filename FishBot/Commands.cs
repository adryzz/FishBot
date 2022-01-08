using Discord;
using System.Diagnostics;
using Anilist4Net;
using Anilist4Net.Enums;
using Discord.Interactions;
using FishBot.Logging;


namespace FishBot
{
    public class SlashCommands : InteractionModuleBase
    {
        [SlashCommand("help", "Gets help", false, RunMode.Async)]
        public async Task Help()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Program.Bot.Config.Color.R, Program.Bot.Config.Color.G, Program.Bot.Config.Color.B);
            embed.WithTitle("FishBot");
            embed.WithUrl("https://github.com/adryzz/fishbot/");
            embed.WithDescription($"fuck you");
            embed.WithCurrentTimestamp();
            await RespondAsync(embed: embed.Build());
        }
        
        
        [SlashCommand("ping", "gets the response times of the server", false, RunMode.Async)]
        public async Task Ping()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int latency = Program.Bot.Latency;
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
            await RespondAsync(embed: embed.Build());
        }

        [SlashCommand("user", "gets info about an AniList user", false, RunMode.Async)]
        public async Task User(string? userName = null)
        {
            try
            {
                User user = await Program.Bot.AnimeClient.GetUserByName(userName ?? Context.User.Username);
                var embed = new EmbedBuilder();
                embed.Author = new EmbedAuthorBuilder {Name = user.Name, Url = user.SiteUrl, IconUrl = user.MediumAvatar };
                embed.Color = user.ProfileColour.ToColor();
                //embed.Description;
                await RespondAsync(embed: embed.Build());
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "User not found.";
                embed.Color = Color.Red;
                await RespondAsync(embed: embed.Build());
                await e.Log();
            }
        }
        
        [SlashCommand("anime", "searches an AniList anime", false, RunMode.Async)]
        public async Task Anime(string name)
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
                    await RespondAsync(embed: embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Anime not found.";
                embed.Color = Color.Red;
                await RespondAsync(embed: embed.Build());
                await e.Log();
            }
        }
        
        [SlashCommand("manga", "searches an AniList manga", false, RunMode.Async)]
        public async Task Manga(string name)
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
                    await RespondAsync(embed: embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Manga not found.";
                embed.Color = Color.Red;
                await RespondAsync(embed: embed.Build());
                await e.Log();
            }
        }
        
        [SlashCommand("media", "searches an AniList media", false, RunMode.Async)]
        public async Task Media(string name)
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
                    await RespondAsync(embed: embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Media not found.";
                embed.Color = Color.Red;
                await RespondAsync(embed: embed.Build());
                await e.Log();
            }
        }
        
        [SlashCommand("animeinfo", "gets additional information about an AniList anime", false, RunMode.Async)]
        public async Task AnimeInfo(string name)
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
                    //embed.AddField(new EmbedFieldBuilder { Name = ":family: Relations", Value = Utils.FormatMediaRelations(result.MediaRelations), IsInline = true });
                    await RespondAsync(embed: embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Anime not found.";
                    embed.Color = Color.Red;
                    await RespondAsync(embed: embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Anime not found.";
                embed.Color = Color.Red;
                await RespondAsync(embed: embed.Build());
                await e.Log();
            }
        }
        
        [SlashCommand("mangainfo", "gets additional information about an AniList anime", false, RunMode.Async)]
        public async Task MangaInfo(string name)
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
                    await RespondAsync(embed: embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.Description = "Manga not found.";
                    embed.Color = Color.Red;
                    await RespondAsync(embed: embed.Build());
                }
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder();
                embed.Description = "Manga not found.";
                embed.Color = Color.Red;
                await RespondAsync(embed: embed.Build());
                await e.Log();
            }
        }
    }
}