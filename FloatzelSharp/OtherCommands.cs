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
            String stats = "```Floatzel Stats\n";
            stats = stats + ".NET Version: " + Environment.Version.ToString() + "\n";
            stats = stats + "OS Version: " + Environment.OSVersion.ToString() + "\n";
            stats = stats + "Processor count: " + Environment.ProcessorCount.ToString() + "\n";
            stats = stats + "64bit OS?: " + Environment.Is64BitOperatingSystem.ToString() + "\n";
            stats = stats + "64bit process?: " + Environment.Is64BitProcess.ToString() + "\n";
            stats = stats + "Memory Usage: " + ((Process.GetCurrentProcess().PrivateMemorySize64 / 1024) / 1024).ToString() + " MB\n";
            stats = stats + "```";
            await ctx.RespondAsync(stats);
        }


    }
}
