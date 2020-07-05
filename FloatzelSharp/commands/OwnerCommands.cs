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

        [Command("admin"), Description("Grant somebody bot admin"), Category(Category.Owner), RequireOwner()]
        public async Task grantadmin(CommandContext ctx, [Description("user whom you wish to make admin")] DiscordMember mem = null) {
            // check if no user is set
            if (mem == null) {
                await ctx.RespondAsync("You did not say who you wish to make bot admin!");
                return;
            }
            // check if they have a profile
            var uid = mem.Id.ToString();
            if (!await Database.dbCheckIfExist(uid)) {
                await Database.dbCreateProfile(uid);
                var prof = await Database.dbLoadProfile(uid);
                prof.admin = true;
                await Database.dbSaveProfile(prof);
            } else {
                var prof = await Database.dbLoadProfile(uid);
                prof.admin = true;
                await Database.dbSaveProfile(prof);
            }
            await ctx.RespondAsync("granted the user admin!");
        }

        [Command("unadmin"), Description("remove someone's admin"), Category(Category.Owner), RequireOwner()]
        public async Task removeAdmin(CommandContext ctx, [Description("user who you wish to remove their admin")] DiscordMember mem = null) {
            // did they say who they want to remove admin from
            if (mem == null) {
                await ctx.RespondAsync("You did not give me a valid discord user whom to remove their admin rights");
                return;
            }
            var uid = mem.Id.ToString();
            var prof = await Database.dbLoadProfile(uid);
            prof.admin = false;
            await Database.dbSaveProfile(prof);
            await ctx.RespondAsync("Admin removed from user");
        }
    }
}
