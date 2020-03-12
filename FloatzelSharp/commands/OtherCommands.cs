using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using FloatzelSharp.help;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System.Diagnostics;

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
        async Task Ping(CommandContext ctx) {
            var msg = await ctx.RespondAsync("Pinging...");
            var ping = msg.Timestamp - ctx.Message.Timestamp;
            var heartbeat = ctx.Client.Ping;
            await msg.ModifyAsync(
                $"🏓 Pong! `{ping.TotalMilliseconds}ms`\n" +
                $"💓 Heartbeat: `{heartbeat}ms`"
            );
        }


    }
}
