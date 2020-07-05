using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using FloatzelSharp.help;
using FloatzelSharp.types;
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
            // check if they have boxes
            var prof = await Database.dbLoadProfile(uid);
            if (!await checkForBox(prof)) {
                await ctx.RespondAsync("You do not own any lootboxes!");
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

        private async Task<bool> checkForBox(Profile prof) {
            if (prof.boxes[0] == 0 && prof.boxes[1] == 0 && prof.boxes[2] == 0 && prof.boxes[3] == 0) {
                return false;
            } else {
                return true;
            }
        }
        private static bool checkForBox(int type, Profile prof) {
            switch (type) {
                case 1:
                    if (prof.boxes[0] == 0) {
                        return false;
                    } else {
                        return true;
                    }
                case 2:
                    if (prof.boxes[1] == 0) {
                        return false;
                    } else {
                        return true;
                    }
                case 3:
                    if (prof.boxes[2] == 0) {
                        return false;
                    } else {
                        return true;
                    }
                case 4:
                    if (prof.boxes[3] == 0) {
                        return false;
                    } else {
                        return true;
                    }
                default:
                    // wait, how the fuck did you end up here?
                    // well, i congratulate you for somehow getting here. Im absolutely amazed
                    // you can have a free lootbox I guess
                    Console.WriteLine("WARNING! Someone managed to enter the defualt path at the Switch Statement starting at line 59 in file LootboxGroup.cs! PANIC!");
                    return true;
            }
        }
        // to avoid copy pasting this same code over and over
        // this handles all the lootbox opening math and random shit
        // deal with it NERDS
        private async Task doLootBox(CommandContext ctx, Profile prof, int min, int max, int type) {
            // do they even have this type of lootbox?
            if (!checkForBox(type, prof)) {
                await ctx.RespondAsync($"You do not own any Tier {type} lootboxes!");
                return;
            }
            // generate random number
            var a = Program.rand.Next(min, max);
            // tell the user what they have won
            await ctx.RespondAsync($"Upon opening the lootbox, you obtained {a}{icon}!");
            // actually give them the money
            prof.bal += a;
            // remove 1 box
            prof.boxes[0] -= 1;
            // save profile
            await Database.dbSaveProfile(prof);
        }

        [Command("open"), Description("used to open owned lootboxes"), Category(Category.Money)]
        public async Task open(CommandContext ctx, [Description("type of lootbox to open. Valid input: t1, t2, t3, t4")] string type = "oof") {
            // get user id
            string uid = ctx.Member.Id.ToString();
            // check if the user in question even has a profile
            if (!await Database.dbCheckIfExist(uid)) {
                await ctx.RespondAsync("You do not own any lootboxes! please go buy some from the shop!");
                return;
            }
            // load profile & check if it has any lootboxes
            var prof = await Database.dbLoadProfile(uid);
            if (!await checkForBox(prof)) {
                await ctx.RespondAsync("you do not own any lootboxes! please buy some with the shop!");
                return;
            }
            // did they say what type of lootbox they want to open?
            if (type.Contains("oof")) {
                await ctx.RespondAsync("You forgot to spesify what type of lootbox you wish to open. Please input either t1, t2, t3 or t4 after the open command!");
                return;
            }
            switch (type) {
                case "t1":
                    await doLootBox(ctx, prof, 1, 40, 1);
                    break;
                case "t2":
                    await doLootBox(ctx, prof, 50, 200, 2);
                    break;
                case "t3":
                    await doLootBox(ctx, prof, 150, 600, 3);
                    break;
                case "t4":
                    await doLootBox(ctx, prof, 250, 1000, 4);
                    break;
                default:
                    await ctx.RespondAsync($"the lootbox type you entered is not valid! Please enter a valid lootbox type next time!");
                    break;

            }
        }
    }
}
