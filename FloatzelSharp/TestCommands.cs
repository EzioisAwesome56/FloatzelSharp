using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FloatzelSharp.help;

namespace FloatzelSharp
{
    class TestCommands : BaseCommandModule
    {
        [Command("test"), Description("a simple test command"), Category(Category.Test)]
        public async Task test(CommandContext ctx)
        {
            await ctx.RespondAsync("Welcome to TESTORZ");
        }
    }
}
