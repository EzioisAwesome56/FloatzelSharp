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
                $"Randomize Tier (r)- 150{icon}\n");
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
                    await ctx.RespondAsync($"You do not have 20{icon} for this box! You only have {prof.bal.ToString()}{icon}");
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
                    await ctx.RespondAsync($"You do not have 100{icon} for this box! You only have {prof.bal.ToString()}{icon}");
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
                    await ctx.RespondAsync($"You do not have 300{icon} for this box! You only have {prof.bal.ToString()}{icon}");
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
                    await ctx.RespondAsync($"You do not have 500{icon} for this box! You only have {prof.bal.ToString()}{icon}");
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
                    await ctx.RespondAsync($"You do not have 150{icon} for this box! You only have {prof.bal.ToString()}{icon}");
                    return;
                }
            }
        }

        [Group("stock"), Description("The one-stop shop for your stock market needs!"), Category(Category.Money)]
        class StockGroup : BaseCommandModule {
            [GroupCommand]
            public async Task shop(CommandContext ctx) {
                await ctx.RespondAsync($"Welcome to the floatzel Stock market!\n" +
                    $"Here are our list of sub commands:\n" +
                    $"```view - View current stock information\n" +
                    $"me - view what stock you currently own (if any)\n" +
                    $"buy - buy a share in a stock\n" +
                    $"sell - sell your share in a stock```");
                return;
            }

            [Command("view"), Description("View the current state of the stock market"), Category(Category.Money)]
            public async Task view(CommandContext ctx) {
                // type
                await ctx.TriggerTypingAsync();
                // is the stock market updating?
                if (!Program.CanStock) {
                    await ctx.RespondAsync("The stock market is currently updating, please wait");
                    return;
                }
                // count how many stocks are in the table
                int count = await Database.dbCountStocks();
                StringBuilder dank = new StringBuilder();
                dank.Append($"```Floatzel Stock Market\n");
                // loop through all the stocks
                for(int i = 1; i <= count; i++) {
                    Stock weed = await Database.dbLoadStock(i.ToString());
                    dank.Append($"{weed.name}-{weed.sid}\n");
                    dank.Append($"Price: {weed.price}{icon}\n");
                    dank.Append($"Difference from last price: {weed.diff}{icon}\n");
                    dank.Append($"Units: {weed.units}\n\n");
                }
                dank.Append("```");
                // send message
                await ctx.RespondAsync(dank.ToString());
            }

            [Command("me"), Description("check the status of your stock"), Category(Category.Money)]
            public async Task me(CommandContext ctx) {
                // check if the user even has a profile to begin with
                if (!await Database.dbCheckIfExist(ctx.Member.Id.ToString())) {
                    // make a profile
                    await Database.dbCreateProfile(ctx.Member.Id.ToString());
                    // since we just made a profile, we have no reason to run any other code
                    await ctx.RespondAsync("You own no stocks!");
                    return;
                }
                // check if the stockmarket is updating
                if (!Program.CanStock) {
                    await ctx.RespondAsync("The stock market is updating, please wait");
                    return;
                }
                var prof = await Database.dbLoadProfile(ctx.Member.Id.ToString());
                // do they even have a stock?
                if (prof.stock[0] == 0) {
                    await ctx.RespondAsync("You own no stocks!");
                    return;
                }
                // load the stock id of the stock that they bought
                var stock = await Database.dbLoadStock(prof.stock[0].ToString());
                StringBuilder a = new StringBuilder();
                a.Append("Your stock profile```\n");
                a.Append($"Name: {stock.name}\n");
                a.Append($"Price at purchase: {prof.stock[1]}{icon}\n");
                a.Append($"Current price: {stock.price}{icon}\n");
                a.Append($"Net gain: {stock.price - prof.stock[1]}```");
                await ctx.RespondAsync(a.ToString());
            }

            [Command("buy"), Description("purchase a share in a stock"), Category(Category.Money)]
            public async Task buy(CommandContext ctx, [Description("id of stock you wish to purchase")] int id = -1) {
                // load this for later use
                var conf = await Config.Get();
                var uid = ctx.Member.Id.ToString();
                // first check if user did not spesify a stock id they would like to purchase
                if (id == -1) {
                    await ctx.RespondAsync($"You did not specify the ID for which stock you want to purchase. Please use \"{(conf.Dev ? conf.Devfix : conf.Prefix)}shop stock view\" to find the correct ID!");
                    return;
                }
                // check if the stock is a valid id
                if (await Database.dbCheckForStock(id.ToString())) {
                    await ctx.RespondAsync($"Provided stock id is INVALID! Please use \"{(conf.Dev ? conf.Devfix : conf.Prefix)}shop stock view\" to find the correct ID!");
                    return;
                }
                // check if the stock market is currently open
                if (!Program.CanStock) {
                    await ctx.RespondAsync("The stock market is currently CLOSED! Please try again later");
                    return;
                }
                // load the stock for future use here
                var stock = await Database.dbLoadStock(id.ToString());
                //  check if there are any units left to purchase
                if (stock.units == 0) {
                    await ctx.RespondAsync("This stock has no more shares left for purchase. Please try again later");
                    return;
                }
                // check if the user has a profile
                if (!await Database.dbCheckIfExist(uid)) {
                    // create blank profile
                    await Database.dbCreateProfile(uid);
                    await ctx.RespondAsync($"You do NOT have the required {stock.price}{icon} to purcahse this stock!");
                    return;
                }
                // load the user's profile for use very soon
                var profile = await Database.dbLoadProfile(uid);
                // check if the user already has a stock
                if (profile.stock[0] != 0) {
                    await ctx.RespondAsync("You already own a stock! please sell it first before you buy another one!");
                    return;
                }
                // check if the user has the money required to buy the stock
                if (profile.bal < stock.price) {
                    await ctx.RespondAsync($"You are {stock.price - profile.bal}{icon} short from being able to afford this stock!");
                    return;
                }
                // looks like all checks have passed. Start the purchase
                // subtract the price from their wallet
                profile.bal -= stock.price;
                // subtract 1 stock unit from the stock itself
                stock.units -= 1;
                // update the profile's stock id and bought price, in that order
                profile.stock[0] = id;
                profile.stock[1] = stock.price;
                // save both stock and profile back to the database
                await Database.dbSaveProfile(profile);
                await Database.dbSaveStock(stock);
                // inform the user
                await ctx.RespondAsync($"You have purchased 1 share in {stock.name} for {stock.price}{icon}!");
            }

            [Command("sell"), Description("Sell your shares for money"), Category(Category.Money)]
            public async Task sell(CommandContext ctx, string firm = "heccu") {
                var conf = await Config.Get();
                // check if they have a profile
                string uid = ctx.Member.Id.ToString();
                if (!await Database.dbCheckIfExist(uid)) {
                    await ctx.RespondAsync("You do not own a stock to sell! Please purchase one before selling");
                    return;
                }
                // is the market open?
                if (!Program.CanStock) {
                    await ctx.RespondAsync("The market is currently closed. Please try again later");
                    return;
                }
                // load profile
                var prof = await Database.dbLoadProfile(uid);
                // load stock
                var stock = await Database.dbLoadStock(prof.stock[0].ToString());
                // did they confirm this action?
                if (!firm.Contains("yes")) {
                    await ctx.RespondAsync($"Are you sure you want to sell your share in {stock.name} for {stock.price}{icon}? Please run \"{(conf.Dev ? conf.Devfix : conf.Prefix)}shop stock sell yes\" to confirm this transaction!");
                    return;
                }
                // update the user profile, first clear out stock information
                prof.stock[0] = 0;
                prof.stock[1] = 0;
                // add the price of the stock to their wallet
                prof.bal += stock.price;
                // add 1 unit to stock
                stock.units += 1;
                // for the hell of it; reduce stock price by 25
                stock.price -= 25;
                // save both things back to the database
                await Database.dbSaveProfile(prof);
                await Database.dbSaveStock(stock);
                // send message
                await ctx.RespondAsync($"You have obtained {stock.price}{icon} for selling your share in {stock.name}!");
            }
        }
    }
}
