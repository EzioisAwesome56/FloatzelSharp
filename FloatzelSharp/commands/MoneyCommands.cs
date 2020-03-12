using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using FloatzelSharp.help;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using FloatzelSharp.util;
using DSharpPlus.Entities;
using FloatzelSharp.types;

namespace FloatzelSharp.commands {
    class MoneyCommands : BaseCommandModule {

        private static string icon = "🥖";


        [Command("bal"), Description("check how much money you or someone else has"), Category(Category.Money), Priority(0)]
        public async Task bal(CommandContext ctx, [Description("Member you want to check (optional)")] DiscordMember dink = null) {
            // get the id of the user
            DiscordMember mem;
            if (dink == null) {
                mem = ctx.Member;
            } else {
                mem = dink;
            }
            string uid = mem.Id.ToString();
            if (await Database.dbCheckIfExist(uid)) {
                var prof = await Database.dbLoadProfile(uid);
                await ctx.RespondAsync($"{mem.Username} has {prof.bal}{icon}");
            } else {
                await ctx.RespondAsync($"{mem.Username} has 0 {icon}");
                await Database.dbCreateProfile(uid);
            }
        }

        [Command("pay"), Description("pay someone some 🥖"), Category(Category.Money)]
        public async Task pay(CommandContext ctx, [Description("member you want to pay money to")] DiscordMember mem = null, [Description("how much you want to pay someone")] int amount = -2) {
            // did they remember to give a user?
            if (mem == null) {
                await ctx.RespondAsync("You didn't give me the user you wanted to pay!");
                return;
            }
            // did they give how much they want to pay?
            if (amount == -2) {
                await ctx.RespondAsync("You didn't tell me how much you want to pay the other user!");
                return;
            }
            // is the amount given a negative number/is 0?
            if (amount < 1) {
                await ctx.RespondAsync($"You cannot pay someone less then 1{icon}");
                return;
            }
            // get ids
            string toId = mem.Id.ToString();
            string fromId = ctx.Member.Id.ToString();
            // check if the person paying even has an account
            if (!await Database.dbCheckIfExist(fromId)) {
                // create an account for them
                await Database.dbCreateProfile(fromId);
                await ctx.RespondAsync($"You do not have {amount.ToString()}{icon} to give!");
                return;
            }
            Profile fromprof = await Database.dbLoadProfile(fromId);
            // check if they have enough money to pay
            if (amount > fromprof.bal) {
                await ctx.RespondAsync($"You only have {fromprof.bal.ToString()}{icon}, not the given {amount.ToString()}{icon} you wish to pay!");
                return;
            }
            // make sure the reciving user has a bank account
             if (!await Database.dbCheckIfExist(toId)) {
                // just make one. no biggie
                await Database.dbCreateProfile(toId);
            }
            // load the person being paid's profile
            Profile toProf = await Database.dbLoadProfile(toId);
            // MATH
            fromprof.bal -= amount;
            toProf.bal += amount;
            // save back to database
            await Database.dbSaveProfile(fromprof);
            await Database.dbSaveProfile(toProf);
            await ctx.RespondAsync($"You paid {amount}{icon} to {mem.Username}!");
        }

        [Command("gamble"), Description("Try your luck to win some money! One attempt costs 5🥖"), Category(Category.Money)]
        public async Task gamble(CommandContext ctx) {
            string uid = ctx.Member.Id.ToString();
            // check if they even have a bank account in the first place
            if (!await Database.dbCheckIfExist(uid)) {
                // make account
                await Database.dbCreateProfile(uid);
                await ctx.RespondAsync($"You do not have 5{icon} to gamble away!");
                return;
            }
            // load balance
            var user = await Database.dbLoadProfile(uid);
            if (user.bal < 5) {
                await ctx.RespondAsync($"you do not have the required 5{icon} to gamble!");
                return;
            }
            // do math
            var rng = Program.rand.Next(0, 20);
            var test = Program.rand.Next(0, 1);
            if (rng <= 15) {
                await ctx.RespondAsync($"YOU LOST! I am gonna enjoy this 5{icon}");
                user.bal -= 5;
                await Database.dbSaveProfile(user);
                return;
            }
            if (rng == 16 || rng == 17 || rng == 18) {
                await ctx.RespondAsync($"YOU WIN! You get your money back and 2{icon} extra");
                user.bal += 2;
                await Database.dbSaveProfile(user);
                return;
            }
            if (rng == 19) {
                await ctx.RespondAsync("YOU WIN! You got triple your bet!");
                user.bal += 5 * 3;
                await Database.dbSaveProfile(user);
                return;
            }
            await ctx.RespondAsync("how the hell did you get here? Ezio's math must suck");
        }

        [Command("loan"), Description("take out an interest-free loan that you don't have to pay back"), Aliases(new string[] { "daily" }), Category(Category.Money)]
        public async Task loan(CommandContext ctx) {
            // generate the amount they will get
            var amount = Program.rand.Next(1, 50);
            // get user id
            string uid = ctx.Member.Id.ToString();
            // check if they have a loan already
            if (!await Database.dbCheckIfExist(uid)) {
                // just give them a loan right off the bat
                await Database.dbCreateProfile(uid, amount, Utils.GetCurrentMilli());
                await ctx.RespondAsync($"You got a loan of {amount}!");
            } else {
                // load profile
                var profile = await Database.dbLoadProfile(uid);
                // do a lot of number magic
                double time = Utils.GetCurrentMilli();
                double pass1 = time - profile.loantime;
                double passed = TimeSpan.FromDays(1).TotalMilliseconds - pass1;


                double hours = TimeSpan.FromMilliseconds(passed).TotalHours;
                // has it been a day?
                if (time != TimeSpan.FromMilliseconds(profile.loantime).Add(TimeSpan.FromDays(1)).TotalMilliseconds && profile.loantime < time && profile.loantime != (double) 0) {
                    await ctx.RespondAsync($"you have to wait {hours.ToString()} more hours before you can get another loan!");
                    return;
                } else {
                    // save new shit
                    profile.bal += amount;
                    profile.loantime = time;
                    await ctx.RespondAsync($"you took out a loan of {amount.ToString()}");
                    await Database.dbSaveProfile(profile);
                    return;
                }
            }
        }
    }
}
