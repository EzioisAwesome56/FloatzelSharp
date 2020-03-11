using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using FloatzelSharp.help;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

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


    }
}
