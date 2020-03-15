using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using FloatzelSharp.help;
using FloatzelSharp.util;
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

        [Command("box"), Priority(0), Description("used to purcahse lootboxes.")]
        public async Task box(CommandContext ctx) {
            await ctx.RespondAsync("We have several lootboxes up for sale\n" +
                $"Tier 1 (t1)- 20{icon}\n" +
                $"Tier 2 (t2)- 100{icon}\n" +
                $"Tier 3 (t3)- 300{icon}\n" +
                $"Tier 4 (t4)- 500{icon}\n" +
                $"Randomize Tier (r)- 150{icon}");
            return;
        }

        [Command("box"), Priority(1), Description("used to purcahse lootboxes.")]
        public async Task box(CommandContext ctx, [Description("the type of box you wish to buy")] string type) { 
            // filter out pings
            type = type.Replace("@everyone", "at everyone").Replace("@here", "at here");
            // check to make sure a valid type was set
            if (!type.Equals("t1") && !type.Equals("t2") && !type.Equals("t3") && !type.Equals("t4") && !type.Equals("r")) {
                await ctx.RespondAsync($"{type} is not a valid lootbox!");
                return;
            }
            string uid = ctx.Member.Id.ToString();
            // do they have a bank account?
            if (!await Database.dbCheckIfExist(uid)) {
                await ctx.RespondAsync($"You do not have enought {icon} for this lootbox!");
                await Database.dbCreateProfile(uid);
                return;
            }
            // load profile
            var prof = await Database.dbLoadProfile(uid);
            // start checking the vaarious lootbox types
            if (type.Equals("t1")) {
                if (prof.bal >= 20) {
                    // subtract cost from bal
                    prof.bal -= 20;
                    // add 1 teir 1 box to profile
                    prof.boxes[0] += 1;
                    // save profile
                    await Database.dbSaveProfile(prof);
                    await ctx.RespondAsync("You bought 1 Tier 1 lootbox!");
                    return;
                } else {
                    await ctx.RespondAsync($"You do not have 20{icon} for this box! You only have {prof.bal.ToString()}");
                    return;
                }
            } else if (type.Equals("t2")) {
                if (prof.bal >= 100) {
                    // subtract cost from bal
                    prof.bal -= 100;
                    // add 1 teir 1 box to profile
                    prof.boxes[1] += 1;
                    // save profile
                    await Database.dbSaveProfile(prof);
                    await ctx.RespondAsync("You bought 1 Tier 2 lootbox!");
                    return;
                } else {
                    await ctx.RespondAsync($"You do not have 100{icon} for this box! You only have {prof.bal.ToString()}");
                    return;
                }
            } else if (type.Equals("t3")) {
                if (prof.bal >= 300) {
                    // subtract cost from bal
                    prof.bal -= 300;
                    // add 1 teir 1 box to profile
                    prof.boxes[2] += 1;
                    // save profile
                    await Database.dbSaveProfile(prof);
                    await ctx.RespondAsync("You bought 1 Tier 3 lootbox!");
                    return;
                } else {
                    await ctx.RespondAsync($"You do not have 300{icon} for this box! You only have {prof.bal.ToString()}");
                    return;
                }
            } else if (type.Equals("t4")) {
                if (prof.bal >= 500) {
                    // subtract cost from bal
                    prof.bal -= 500;
                    // add 1 teir 1 box to profile
                    prof.boxes[3] += 1;
                    // save profile
                    await Database.dbSaveProfile(prof);
                    await ctx.RespondAsync("You bought 1 Tier 4 lootbox!");
                    return;
                } else {
                    await ctx.RespondAsync($"You do not have 500{icon} for this box! You only have {prof.bal.ToString()}");
                    return;
                }
            } else if (type.Equals("r")) {
                if (prof.bal >= 150) {
                    // subtract cost from bal
                    prof.bal -= 150;
                    await ctx.RespondAsync("You bought 1 Random Tier lootbox!");
                    // rng
                    var rng = Program.rand.Next(20);
                    if (rng <= 13) {
                        await ctx.RespondAsync("You got a Tier 1 lootbox!");
                        prof.boxes[0] += 1;
                    } else if (rng == 14 || rng == 15 || rng == 16) {
                        await ctx.RespondAsync("You got a Tier 2 lootbox!");
                        prof.boxes[1] += 1;
                    } else if (rng == 17 || rng == 18) {
                        await ctx.RespondAsync("You got a Tier 3 lootbox!");
                        prof.boxes[2] += 1;
                    } else {
                        await ctx.RespondAsync("You got a Tier 4 lootbox!");
                        prof.boxes[3] += 1;
                    }
                    // save profile
                    await Database.dbSaveProfile(prof);
                    return;
                } else {
                    await ctx.RespondAsync($"You do not have 150{icon} for this box! You only have {prof.bal.ToString()}");
                    return;
                }
            }
        }
    }
}
