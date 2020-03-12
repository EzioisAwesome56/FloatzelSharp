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

        [Command("bal"), Description("check how much money you or someone else has"), Category(Category.Money), Priority(0)]
        public async Task bal(CommandContext ctx, [Description("Member you want to check")] DiscordMember dink = null) {
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
                await ctx.RespondAsync(mem.Username + " has 0" + icon);
                // TODO: save a 0
            }
        }
    }
}
