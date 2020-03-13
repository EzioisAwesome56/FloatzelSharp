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

        public static string version = "3.0";
        public static Random rand = new Random();

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
            // check if we have old tweets to convert
            if (Database.dbCheckForOldTweets()) {
                Database.dbConvertTweets();
            }
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
                StringPrefixes = new string[] { config.Dev ? config.Devfix : config.Prefix },
                EnableDefaultHelp = false,
                IgnoreExtraArguments = true
            });
            commands.CommandErrored += PrintError;
            commands.RegisterCommands<OtherCommands>();
            commands.RegisterCommands<TestCommands>();
            commands.RegisterCommands<HelpCmd>();
            commands.RegisterCommands<MoneyCommands>();
            commands.RegisterCommands<FunCommands>();
            commands.RegisterCommands<OwnerCommands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);

        }
    }
}
