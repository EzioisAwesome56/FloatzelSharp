using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using FloatzelSharp.util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FloatzelSharp.attribute {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    class RequireAdminAttribute : CheckBaseAttribute {

        private static string error = "Error: This command can only be run by bot owners or admins";

        public RequireAdminAttribute() { }
        
        public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) {
            // check if user has a profile
            string uid = ctx.Member.Id.ToString();
            if (!await Database.dbCheckIfExist(uid)) {
                await ctx.RespondAsync(error);
                return false;
            }
            // load user profile
            var prof = await Database.dbLoadProfile(uid);
            // is the user an admin?
            if (prof.admin) {
                return true;
            } else {
                await ctx.RespondAsync(error);
                return false;
            }
        }
    }
}
