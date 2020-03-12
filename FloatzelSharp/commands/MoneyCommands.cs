using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using FloatzelSharp.help;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using FloatzelSharp.util;
using DSharpPlus.Entities;

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
            if (Database.dbCheckIfExist(uid)) {
                await ctx.RespondAsync($"{mem.Username} has {Database.dbLoadInt(uid).ToString()}{icon}");
            } else {
                await ctx.RespondAsync($"{mem.Username} has 0 {icon}");
                Database.dbCreateAccount(uid);
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
            if (!Database.dbCheckIfExist(fromId)) {
                // create an account for them
                Database.dbCreateAccount(fromId);
                await ctx.RespondAsync($"You do not have {amount.ToString()}{icon} to give!");
                return;
            }
            int frombal = Database.dbLoadInt(fromId);
            // check if they have enough money to pay
            if (amount > frombal) {
                await ctx.RespondAsync($"You only have {frombal.ToString()}{icon}, not the given {amount.ToString()}{icon} you wish to pay!");
                return;
            }
            // make sure the reciving user has a bank account
             if (!Database.dbCheckIfExist(toId)) {
                // just make one. no biggie
                Database.dbCreateAccount(toId);
            }
            // load tobal
            int toBal = Database.dbLoadInt(toId);
            // MATH
            frombal -= amount;
            toBal += amount;
            // save back to database
            Database.dbSaveInt(fromId, frombal);
            Database.dbSaveInt(toId, toBal);
            await ctx.RespondAsync($"You paid {amount}{icon} to {mem.Username}!");
        }

        [Command("gamble"), Description("Try your luck to win some money! One attempt costs 5🥖"), Category(Category.Money)]
        public async Task gamble(CommandContext ctx) {
            string uid = ctx.Member.Id.ToString();
            // check if they even have a bank account in the first place
            if (!Database.dbCheckIfExist(uid)) {
                // make account
                Database.dbCreateAccount(uid);
                await ctx.RespondAsync($"You do not have 5{icon} to gamble away!");
                return;
            }
            // load balance
            var bal = Database.dbLoadInt(uid);
            // do math
            var rng = Program.rand.Next(0, 20);
            var test = Program.rand.Next(0, 1);
            if (rng <= 15) {
                await ctx.RespondAsync($"YOU LOST! I am gonna enjoy this 5{icon}");
                Database.dbSaveInt(uid, bal - 5);
                return;
            }
            if (rng == 16 || rng == 17 || rng == 18) {
                await ctx.RespondAsync($"YOU WIN! You get your money back and 2{icon} extra");
                Database.dbSaveInt(uid, bal + 2);
                return;
            }
            if (rng == 19) {
                await ctx.RespondAsync("YOU WIN! You got triple your bet!");
                Database.dbSaveInt(uid, bal += 5 * 3);
                return;
            }
            await ctx.RespondAsync("how the hell did you get here? Ezio's math must suck");
        }
    }
}
