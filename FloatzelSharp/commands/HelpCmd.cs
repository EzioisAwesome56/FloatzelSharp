﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using FloatzelSharp.kekbot;
using FloatzelSharp.help;

namespace FloatzelSharp.commands {
    class HelpCmd : BaseCommandModule {

        private const string tagline = "FloatzelSharp: the big rewrite";

        [Command("help"), Description("You're already here, aren't you?"), Category(Category.Help)]
        async Task Help(
            CommandContext ctx,
            [RemainingText, Description("The command or category to look for.")] string query = ""
        ) {
            if (query.Length == 0) {
                //If no arguments were given, bring the commands list.
                await DisplayCategoryHelp(ctx);
            } else if (ctx.CommandsNext.FindCommand(query, out var _) is Command cmd) {
                //Print the command help, if the command has been found.
                await DisplayCommandHelp(ctx, cmd);
            } else if (Enum.TryParse(query, true, out Category cat)) {
                //Command wasn't found, is it a category?
                await DisplayCategoryHelp(ctx, cat);
            } else {
                //the answer is no
                await ctx.RespondAsync("Command/Category not found.");
            }
        }

        private static async Task DisplayCommandHelp(CommandContext ctx, Command cmd) {
            //Setup the embed.
            var aliases = string.Join(", ", cmd.Aliases.Select(alias => $"`{alias}`"));
            var embed = new DiscordEmbedBuilder()
                .WithTitle($"`{cmd.Name}`" + (aliases.Length > 0 ? $" (or {aliases})" : ""))
                .WithDescription(cmd.Description)
                .WithAuthor(name: tagline, iconUrl: ctx.Client.CurrentUser.AvatarUrl);
            //Prepare ourselves for usage
            var usage = new StringBuilder();
            //The total count of subcommands and overloads.
            //LITERALLY THE CMD IS COMMANDGROUP THING IS ONLY HERE BECAUSE VS HATED WHEN I TRIED TO DO ANYTHING ELSE.
            var count = cmd.Overloads.Count + (cmd is CommandGroup ? (cmd as CommandGroup).Children.Count : 0);

            //The following loop handles overloads.
            foreach (var ovrld in cmd.Overloads) {
                if (ovrld.Priority >= 0)
                    AppendOverload(ovrld);
            }

            //Do we have any subcommands?
            if (cmd is CommandGroup group) {
                //The following loop handles subcommands and their appropriate usage.
                foreach (var subcmd in group.Children) {
                    if (subcmd.Overloads.Count > 1 || subcmd is CommandGroup)
                        usage.AppendLine($"`{cmd.Name} {subcmd.Name}`: Visit `help {cmd.Name} {subcmd.Name}` for more information.");
                    else
                        AppendOverload(subcmd.Overloads.Single(), subcmd);
                }
            }

            //I heard you like methods, so I put a method in your method.
            void AppendOverload(CommandOverload ovrld, Command? subcmd = null) {
                var ovrldHasArgs = ovrld.Arguments.Count > 0;

                usage.Append("`");
                usage.Append(cmd.Name);
                if (subcmd != null) usage.Append($" {subcmd.Name}");
                //Make sure we actually have arguments, otherwise don't bother adding a space for them.
                if (ovrldHasArgs) {
                    usage.Append(" ");
                    //We have arguments, let's print them.
                    usage.AppendJoin(" ", ovrld.Arguments.Select(arg => (arg.IsOptional && !arg.IsCustomRequired()) ? $"({arg.Name})" : $"[{arg.Name}]"));
                }
                usage.Append("`");

                if (subcmd != null) usage.Append($": {subcmd.Description}");
                usage.AppendLine();
                //Is the count of subcommands and overloads short?
                if (count <= 6) {
                    //Second argument loop for descriptions (if count is short)
                    if (ovrldHasArgs)
                        usage.AppendLines(ovrld.Arguments.Select(arg => $"`{arg.Name}`: {arg.Description}"));
                    usage.AppendLine();
                }
            }

            embed.AddField("Usage:", usage.ToString(), inline: false);
            await ctx.RespondAsync(embed: embed);
        }

        private static async Task DisplayCategoryHelp(CommandContext ctx, Category? catOrAll = null) {
            var paginator = new EmbedPaginator(ctx.Client.GetInteractivity()) {
                ShowPageNumbers = true
            };
            paginator.Users.Add(ctx.Member.Id);

            paginator.Embeds.AddRange(catOrAll is Category cat
                ? GetCategoryPages(ctx, cat)
                : Enum.GetValues(typeof(Category)).Cast<Category>().SelectMany(cat => GetCategoryPages(ctx, cat)));

            await paginator.Display(ctx.Channel);
        }

        private static IEnumerable<DiscordEmbed> GetCategoryPages(CommandContext ctx, Category cat) {
            var cmds = ctx.CommandsNext.RegisteredCommands.Values
                .Where(c => c.GetCategory() == cat)
                .OrderBy(c => c.Name)
                .Distinct()
                .ToList();
            return Util.Range(end: cmds.Count, step: 10)
                .Select(i => new DiscordEmbedBuilder()
                    .WithTitle(Enum.GetName(typeof(Category), cat))
                    .WithDescription(string.Join("\n", cmds
                        .GetRange(i, Math.Min(i + 10, cmds.Count))
                        .Select(c => $"{c.Name} - {c.Description}")))
                    .WithAuthor(tagline, iconUrl: ctx.Client.CurrentUser.AvatarUrl)
                    .WithFooter("Floatzel 3.0")
                    .Build());
        }

    }
}