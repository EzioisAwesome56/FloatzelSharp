using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FloatzelSharp
{
    class TestCommands : BaseCommandModule
    {
        [Command("test")]
        public async Task test(CommandContext ctx)
        {
            await ctx.RespondAsync("Welcome to TESTORZ");
        }
    }
}
