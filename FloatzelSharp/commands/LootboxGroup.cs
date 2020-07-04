using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using FloatzelSharp.help;
using FloatzelSharp.util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FloatzelSharp.commands {
    [Group("lootbox"), Description("open up some lootboxes to get prizes!"), Category(Category.Money)]
    class LootboxGroup : BaseCommandModule {

        // this will be important later
        private static string icon = "🥖";

        [GroupCommand]
        public async Task lootbox(CommandContext ctx) {
            // TODO: do cool things here
            await ctx.RespondAsync("this command doesnt work you fool!");
        }

        [Command("view"), Description("view how many lootboxes you have"), Category(Category.Money)]
        public async Task view(CommandContext ctx) {
            // obtain user id
            string uid = ctx.Member.Id.ToString();
            // check if they have a profile
            if (!await Database.dbCheckIfExist(uid)) {
                // make a blank profile
                await Database.dbCreateProfile(uid);
                // fake out the user
                await ctx.RespondAsync("You do not own any lootboxes!");
                return;
            }
            // load the user's profile
            var prof = await Database.dbLoadProfile(uid);
        }
    }
}
