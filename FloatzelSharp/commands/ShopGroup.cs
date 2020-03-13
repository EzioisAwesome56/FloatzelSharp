using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using FloatzelSharp.help;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FloatzelSharp.commands {
    [Group("shop"), Description("the go-to place for spending your bread!"), Category(Category.Money)]
    class ShopGroup : BaseCommandModule {

        private static string icon = "🥖";

        [GroupCommand]
        public async Task shop(CommandContext ctx) {
            await ctx.RespondAsync($"List shop groups here...somehow?");
            return;
        }

        [Command("box"), Priority(1), Description("used to purcahse lootboxes.")]
        public async Task box(CommandContext ctx) {
            await ctx.RespondAsync("We have several lootboxes up for sale\n" +
                $"Teir 1 (t1)- 20{icon}\n" +
                $"Teir 2 (t2)- 100{icon}\n" +
                $"Teir 3 (t3)- 300{icon}\n" +
                $"Teir 4 (t4)- 500{icon}\n" +
                $"Randomize Tier (r)- 150{icon}");
            return;
        }

        [Command("box"), Priority(0), Description("used to purcahse lootboxes.")]
        public async Task box(CommandContext ctx, [Description("the type of box you wish to buy")] string type = null) {
            // check if they even gave a type of box
            if (type == null) {
                await ctx.RespondAsync("You forgot to say what type of box you wish to buy!");
                return;
            }
            // check to make sure a valid type was set
            if (type != "t1" || type != "t2" || type != "t3" || type != "t4" || type != "r") {
                await ctx.RespondAsync($"{type} is not a valid lootbox!");
                return;
            }
        }
    }
}
