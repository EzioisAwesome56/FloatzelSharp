using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Interactivity;
using FloatzelSharp.commands;
using FloatzelSharp.util;

namespace FloatzelSharp
{
    class Program
    {

        private async static Task PrintError(CommandErrorEventArgs e) {
            if (e.Exception is CommandNotFoundException) return;
            await e.Context.Channel.SendMessageAsync($"An error occured: {e.Exception.Message}");
            Console.Error.WriteLine(e.Exception);
        }

        static DiscordClient discord;
        static CommandsNextExtension commands;
        static InteractivityExtension? Interactivity;
        static void Main(string[] args)
        {
            Database.dbinit();
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var config = await Config.Get();
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = config.Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            Interactivity = discord.UseInteractivity(new InteractivityConfiguration());

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { config.Dev ? config.Prefix : config.Devfix },
                EnableDefaultHelp = false
            });
            commands.CommandErrored += PrintError;
            commands.RegisterCommands<OtherCommands>();
            commands.RegisterCommands<TestCommands>();
            commands.RegisterCommands<HelpCmd>();
            commands.RegisterCommands<MoneyCommands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);

        }
    }
}
