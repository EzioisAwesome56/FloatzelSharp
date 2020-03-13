using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
    }
}
