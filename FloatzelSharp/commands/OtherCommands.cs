using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using FloatzelSharp.help;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System.Diagnostics;
using FloatzelSharp.attribute;
using FloatzelSharp.util;
using FloatzelSharp.types;

namespace FloatzelSharp
{
    class OtherCommands : BaseCommandModule
    {
        [Command("pi"), Description("Tells you what pi is"), Category(Category.Other)]
        public async Task pi(CommandContext ctx)
        {
            await ctx.RespondAsync("pi is " + Math.PI.ToString());
        }
        [Command("random"), Description("generates a random number"), Category(Category.Other)]
        public async Task random(CommandContext ctx)
        {
            Random rand = new Random();
            await ctx.RespondAsync(rand.Next().ToString());
        }
        [Command("stats"), Description("shows states about the bot"), Category(Category.Other)]
        public async Task stats(CommandContext ctx)
        {
            await ctx.RespondAsync($"```Floatzel Stats\n" +
                $"Floatzel Version: {Program.version}\n" +
                $".NET Version: {Environment.Version.ToString()}\n" +
                $"OS Version: {Environment.OSVersion.ToString()}\n" +
                $"Processor Count: {Environment.ProcessorCount.ToString()}\n" +
                $"Is 64Bit OS: {Environment.Is64BitOperatingSystem.ToString()}\n" +
                $"Is 64Bit Process: {Environment.Is64BitProcess.ToString()}\n" +
                $"Memory Usage: {((Process.GetCurrentProcess().PrivateMemorySize64 / 1024) / 1024).ToString()} MB```");
        }

        [Command("ping"), Description("Returns with the bot's ping."), Aliases("pong")]
        public async Task Ping(CommandContext ctx) {
            var msg = await ctx.RespondAsync("Pinging...");
            var ping = msg.CreationTimestamp - ctx.Message.CreationTimestamp;
            var heartbeat = ctx.Client.Ping;
            await msg.ModifyAsync(
                $"🏓 Pong! `{ping.TotalMilliseconds}ms`\n" +
                $"💓 Heartbeat: `{heartbeat}ms`"
            );
        }

        [Command("invite"), Description("posts a link to invite the bot"), Category(Category.Other)]
        public async Task invite(CommandContext ctx) {
            var oof = await Config.Get();
            await ctx.RespondAsync($"This bot's invite link is {oof.Invite}");
        }

        [Command("addtweet"), Description("Add a tweet to the tweet pool (admin only!)"), Category(Category.Other), RequireAdmin()]
        public async Task addtweet(CommandContext ctx, [Description("text for the tweet you want to add")] string content = "gfshjkadllhjjdsfi7") {
            // did they even say what they wanted to add?
            if (content.Contains("gfshjkadllhjjdsfi7")) {
                await ctx.RespondAsync("You did not say what you wanted to add as a tweet!");
                return;
            }
            // get the current tweet total
            var total = await Database.dbCountTweets();
            // make a new tweet
            Tweet bird = new Tweet {
                tid = (total + 1).ToString(),
                txt = content
            };
            // save tweet
            await Database.dbSaveTweet(bird);
            await ctx.RespondAsync("Tweet saved!");
        }

        [Command("counttweets"), Description("count how many tweets floatzel has"), Category(Category.Other)]
        public async Task counttweets(CommandContext ctx) {
            // load total amount from database
            var total = await Database.dbCountTweets();
            await ctx.RespondAsync($"There are a total of {total} tweets in the database.");
        }

        [Command("tweet"), Description("post something to Floatzel's Twitter acc"), Category(Category.Other), RequireAdmin()]
        public async Task tweet(CommandContext ctx, [Description("the content you wish to tweet")] string content = "gfshjkadllhjjdsfi7") {
            // did they even put what they want
            if (content.Contains("gfshjkadllhjjdsfi7")) {
                await ctx.RespondAsync("you did not spesify what you wish to tweet");
                return;
            }
            // TO-DO: FINISH THIS
        }

    }
}
