using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using FloatzelSharp.help;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FloatzelSharp.commands {
    class FunCommands : BaseCommandModule {

        [Command("8ball"), Description("Ask the 8ball something"), Category(Category.Fun)]
        public async Task ball(CommandContext ctx, [Description("what you wish to ask the 8ball"), RemainingText()] string question = null) {
            // check if they forgot to provide an answer
            if (question == null) {
                await ctx.RespondAsync("You forgot to provide a question silly!");
                return;
            }
            string[] replies = new string[] { "I say no!", "I don't care", "I do not understand!", "Sure thing pal!", "The star align for you!", "Please try again!", "8ball.exe has crashed",
                "Why did you ask me this?", "NOPE!", "Commit yes.exe" };
            await ctx.RespondAsync($"You asked- {question}\n" +
                $"🎱 Response- {replies[Program.rand.Next(replies.Length)]}");
            return;
        }
    }
}
