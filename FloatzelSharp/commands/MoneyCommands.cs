using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using FloatzelSharp.help;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using FloatzelSharp.util;

namespace FloatzelSharp.commands {
    class MoneyCommands : BaseCommandModule {

        private static string icon = "🥖";

        private static bool checkBank(string id) {
            if (!Database.dbCheckIfExist(id)) {
                // save a 0
                // TODO: this
                // return false
                return false;
            } else {
                return true;
            }
        }

        [Command("bal"), Description("check how much money you or someone else has"), Category(Category.Money)]
        public async Task bal(CommandContext ctx) {
            string uid = ctx.Member.Id.ToString();
            if (!checkBank(uid)) {
                await ctx.RespondAsync("You have 0" + icon);
                return;
            }
            // TODO: load db value.
            var dank = Database.dbLoadInt(uid);
            foreach (var i in dank) {
                Console.WriteLine(i);
            }
        }
    }
}
