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
            // check if they have any lootboxes at all
            if (prof.boxes[0] == 0 && prof.boxes[1] == 0 && prof.boxes[2] == 0 && prof.boxes[3] == 0) {
                await ctx.RespondAsync("You own no lootboxes! Go buy some and try again!");
                return;
            }
            // produce fancy display
            await ctx.RespondAsync($"```These are your current lootboxes:\n\n" +
                $"Tier 1 boxes: {prof.boxes[0]}\n" +
                $"Tier 2 boxes: {prof.boxes[1]}\n" +
                $"Tier 3 boxes: {prof.boxes[2]}\n" +
                $"Tier 4 boxes: {prof.boxes[3]}```");
            return;
        }
    }
}
