﻿using DSharpPlus.CommandsNext;
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
            question = question.Replace("@everyone", "at everyone").Replace("@here", "at here");
            string[] replies = new string[] { "I say no!", "I don't care", "I do not understand!", "Sure thing pal!", "The star align for you!", "Please try again!", "8ball.exe has crashed",
                "Why did you ask me this?", "NOPE!", "Commit yes.exe" };
            await ctx.RespondAsync($"You asked- {question}\n" +
                $"🎱 Response- {replies[Program.rand.Next(replies.Length)]}");
            return;
        }

        [Command("dice"), Description("rolls a dice for you"), Category(Category.Fun)]
        public async Task roll(CommandContext ctz) {
            var roll = Program.rand.Next(6) + 1;
            await ctz.RespondAsync($"You rolled a {roll.ToString()}");
        }

        [Command("eat"), Description("I will evaluate whatever you give me to eat"), Category(Category.Fun)]
        public async Task eat(CommandContext ctx, [Description("What I shall eat"), RemainingText()] string food = null) {
            if (food == null) {
                await ctx.RespondAsync("You forgot to say what you want me to eat!");
                return;
            }
            food = food.Replace("@everyone", "at everyone").Replace("@here", "at here");
            string[] taste = new string[] { "Chicken", "Pizza", "Fried Shrimp", "Blood", "Fried Butter", "Dank Memes", "Oxygen on crack", "Milk", "Pork" };
            await ctx.RespondAsync($"You gave me- {food}\n" +
                $"Rating- {Program.rand.Next(11).ToString()}/10\n" +
                $"Tastes like- {taste[Program.rand.Next(taste.Length)]}");
            return;
        }

        [Command("bf"), Description("Rates your boyfriend"), Aliases(new string[] { "boyfriend"}), Category(Category.Fun)]
        public async Task bf(CommandContext ctx, [Description("name of who you want me to rate"), RemainingText()] string name = null) {
            if (name == null) {
                await ctx.RespondAsync("You forgot to tell me the name of whom you want me to judge!");
                return;
            }
            name = name.Replace("@everyone", "at everyone").Replace("@here", "at here");
            await ctx.RespondAsync($"You asked me to rate- {name}\n" +
                $"Rating- {Program.rand.Next(11).ToString()}/10\n" +
                $"Do I approve- {(Program.rand.Next(2).Equals(1) ? "Yes" : "No")}");
            return;
        }

        [Command("gf"), Description("Rates your girlfriend"), Aliases(new string[] { "girlfriend" }), Category(Category.Fun)]
        public async Task gf(CommandContext ctx, [Description("name of who you want me to rate"), RemainingText()] string name = null) {
            if (name == null) {
                await ctx.RespondAsync("You forgot to tell me the name of whom you want me to judge!");
                return;
            }
            name = name.Replace("@everyone", "at everyone").Replace("@here", "at here");
            await ctx.RespondAsync($"You asked me to rate- {name}\n" +
                $"Rating- {Program.rand.Next(11).ToString()}/10\n" +
                $"Do I approve- {(Program.rand.Next(2).Equals(1) ? "Yes" : "No")}");
            return;
        }
    }
}
