using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;

namespace FloatzelSharp
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextExtension commands;
        static InteractivityExtension? Interactivity;
        static void Main(string[] args)
        {
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
            commands.RegisterCommands<OtherCommands>();
            commands.RegisterCommands<TestCommands>();
            commands.RegisterCommands<HelpCmd>();

            await discord.ConnectAsync();
            await Task.Delay(-1);

        }
    }
}
