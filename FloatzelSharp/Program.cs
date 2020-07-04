using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Lavalink;
using FloatzelSharp.commands;
using FloatzelSharp.kekbot;
using FloatzelSharp.util;
using Microsoft.Extensions.DependencyInjection;

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
        // timer for game status stuff
        private static Timer GameTimer = null;
        // timer for stock market updates
        private static Timer StockTimer = null;
        public static bool CanStock = true;

        // i think we need this for lavalink
        static LavalinkExtension Lavalink { get; set; }
        static IServiceProvider Services { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine($"Floatzel Version {version} now starting up...");
            Database.dbinit();
            // check if we have old tweets to convert
            if (Database.dbCheckForOldTweets()) {
                Database.dbConvertTweets();
            }
            if (Database.dbCheckForOldStocks()) {
                Database.dbConvertStocks();
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

            // thingy for game status
            discord.Ready += OnReady;

            Interactivity = discord.UseInteractivity(new InteractivityConfiguration());

            Lavalink = discord.UseLavalink();

            // services shit
            // mostly for lavaplayer
            Services = new ServiceCollection()
                .AddSingleton<MusicService>()
                .AddSingleton(new LavalinkService(discord))
                .BuildServiceProvider(true);

            commands = discord.UseCommandsNext(new CommandsNextConfiguration {
                StringPrefixes = new string[] { config.Dev ? config.Devfix : config.Prefix },
                EnableDefaultHelp = false,
                IgnoreExtraArguments = true,
                Services = Services
            });
            commands.CommandErrored += HandleError;
            commands.RegisterCommands<OtherCommands>();
            commands.RegisterCommands<TestCommands>();
            commands.RegisterCommands<HelpCmd>();
            commands.RegisterCommands<MoneyCommands>();
            commands.RegisterCommands<FunCommands>();
            commands.RegisterCommands<OwnerCommands>();
            commands.RegisterCommands<ShopGroup>();
            commands.RegisterCommands<ImageCommands>();
            commands.RegisterCommands<MusicCommands>();
            commands.RegisterCommands<LootboxGroup>();

            await discord.ConnectAsync();
            await Task.Delay(-1);

        }

        // copy-paste from KekbotSharp, with a few minor edits cuz im not sharding
        private static Task OnReady(ReadyEventArgs e) {
            Console.WriteLine("Floatzel is ready to serve!");

            if (GameTimer == null) {
                GameTimer = new Timer(GameTimerCallback, e.Client, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            }
            if (StockTimer == null) {
                StockTimer = new Timer(StockUpdater, 0, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            }
            return Task.CompletedTask;
        }

        // loosely based on kekbotsharp
        // does shit
        private static async Task HandleError(CommandErrorEventArgs errorArgs) {
            var error = errorArgs.Exception;
            var ctx = errorArgs.Context;
            if (error is CommandCancelledException) {
                return;
            }
            await PrintError(errorArgs);
        }


        // copy-pasted function from kekbotSharp, but heavily modifyed to suit my needs
        // thanks, jorge!
        private static async void GameTimerCallback(object _) {
            var client = _ as DiscordClient;
            try {
                DiscordActivity dank = null;
                var rng = rand.Next(3);
                switch (rng) {
                    case 0:
                        dank = new DiscordActivity(games[rand.Next(games.Length)], ActivityType.ListeningTo);
                        break;
                    case 1:
                        dank = new DiscordActivity(games[rand.Next(games.Length)], ActivityType.Playing);
                        break;
                    case 2:
                        dank = new DiscordActivity(games[rand.Next(games.Length)], ActivityType.Watching);
                        break;
                }
                await client.UpdateStatusAsync(dank, UserStatus.Online);
                Console.WriteLine($"Presense updated to {dank.ActivityType.ToString()} - {dank.Name.ToString()} at {DateTime.Now.ToString()}");
                GC.Collect();
            } catch (Exception e) {
                Console.WriteLine("ERROR UPDATING STATUS!!!!!");
            }
        }

            // tiimer function to update le stock market
            private static async void StockUpdater(object a) {
            // init the update shit
            CanStock = false;
            Console.WriteLine("Floatzel is now updating the stocks...");
            // get how many stocks are present
            var total = await Database.dbCountStocks();
            var count = 1;
            // loop time OOF
            while (total >= count) {
                // step 1: load the first stock
                var stock = await Database.dbLoadStock(count.ToString());
                // step 2: generate how much to add/subtract
                var rng = rand.Next(500);
                // step 3: add or subtract?
                var type = rand.Next(2);
                // step 4: actually do the thing
                if (type == 0) {
                    stock.price += rng;
                    stock.diff = rng;
                } else {
                    stock.price -= rng;
                    stock.diff = -rng;
                }
                // step 4.5: check if the new price if below 0
                if (stock.price < 0) {
                    stock.price = 1;
                }
                // step 5: save the stock back to the database
                await Database.dbSaveStock(stock);
                // step 6 (optional): report stock has been updated
                Console.WriteLine($"Stock id {count} has been updated");
                // step 7: inc the counter
                count++;
            }
            // we are done updating, re open the market
            CanStock = true;
            Console.WriteLine("Floatzel has finished updating all the stocks.");
        }


        // copy-paste from Floatzel: Java Edition. GAMES
        // imo they are too iconic to be thrown away entirely
        static string[] games = { "Windows 7", "mspaint.exe", "&help", "Nintendo Gamecube", "GIMP startup",
            "fucking weed", "Sonic 09", "Dying in a hole", "gay xd", "Exploding North Korea", "Being a terrorist", "BSOD",
            "Extra gay", "memerino and the dank machine", "Dankform", "Half Life 4", "18 is a god", "SaltyPepper", "@here",
            "Simpsons", "Explosive cheese", "Being a bot", "hi Cublex", "Pentium 3", "meme machine", "18 is bisexual", "ezio is bi",
            "PyCharm", "Rectangle Gay", "Making the frogs gay", "eirrac", "what the fuck is a hedgehog", "heck you blur", "enitsirhc", "fuck me harder daddy",
            "Intelij", "Smoking weed", "Persona -12 + GAY DLC", "Circles", "Godson is radical", "Ryan is gay", "Java 10", "with Yukari", "Super Dankio Ocyssey",
            "Fucking Mitsuru with a stick", "Persona: Waifu simulator", "Ezio X Yukari", "Yukari is hot", "Ezio is dumb", "Smelling Ralsei's feet", "Licking Ralsei feetz",
            "Calling 18 a gaylord", "Watching Mitsuru and Ralsei fuck", "DMAN has the largest gay", "TOUHOU", "Nintendo", "Linux, bitch", "Mozilla Chrome", "Shooting Internet Explorer", "hi esmbot",
            "Cirno is dumb lol"};

    }
}
