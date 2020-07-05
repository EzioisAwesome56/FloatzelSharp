using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using FloatzelSharp.help;
using FloatzelSharp.util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FloatzelSharp.commands {
    class OwnerCommands : BaseCommandModule {

        [Command("inflate"), Description("fill your wallet with cash"), Category(Category.Owner), RequireOwner()]
        public async Task inflate(CommandContext ctx) {
            // check if profile doesnt exist
            if (!await Database.dbCheckIfExist(ctx.Member.Id.ToString())) {
                await Database.dbCreateProfile(ctx.Member.Id.ToString(), int.MaxValue);
            } else {
                // load profile
                var dank = await Database.dbLoadProfile(ctx.Member.Id.ToString());
                dank.bal = int.MaxValue;
                // save it
                await Database.dbSaveProfile(dank);
            }
            await ctx.RespondAsync("Your wallet has been filled with cash");
        }

        [Command("delete"), Description("Delete someone's profile"), Category(Category.Owner), RequireOwner()]
        public async Task deletep(CommandContext ctx, [Description("member who's profile you would like to delete")] DiscordMember mem = null) {
            if (mem == null) {
                await ctx.RespondAsync("You did not say who you wish to delete!");
                return;
            }
            // delete their profile
            await Database.dbDeleteProfile(mem.Id.ToString());
            await ctx.RespondAsync("Profile deleted!");
        }
    }
}
