using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using System.Net;
using System.IO;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using FloatzelSharp.help;
using DSharpPlus.Entities;

namespace FloatzelSharp.commands {
    class ImageCommands : BaseCommandModule {

        private async Task<string> GetPastAttachment(CommandContext ctx) {
            string url = null;
            var dank = await ctx.Channel.GetMessagesAsync(10);
            for (var i = 0; i <= 9; i++) {
                if (dank[i].Attachments.Count > 0) {
                    url = dank[i].Attachments[0].Url;
                    break;
                }
            }
            return url;

        }

        [Command("imagetest"), Description("this command exists to make sure "), Category(Category.Test)]
        public async Task dank(CommandContext ctx) {
            await ctx.TriggerTypingAsync();

            using (var client = new WebClient()) {
                Stream stream = await client.OpenReadTaskAsync(new Uri(ctx.User.AvatarUrl));
                IMagickImage ava = new MagickImage(stream);
                Stream streamm = new MemoryStream();
                IMagickImage img = new MagickImage(MagickColor.FromRgba(255, 0, 0, 255), ava.Width, ava.Height);
                img.Format = MagickFormat.Png64;
                img.Composite(ava, 50, 50, CompositeOperator.SrcOver);
                var memory = new MemoryStream(img.ToByteArray());
                await ctx.RespondWithFileAsync("test.png", memory, "test");
                // Dispose of everything used
                await memory.DisposeAsync();
                await stream.DisposeAsync();
                img.Dispose();
            }
        }

        [Command("wall"), Description("make a wall from an attachment"), Category(Category.Image)]
        public async Task wall(CommandContext ctx) {
            // type a thing(tm)
            await ctx.TriggerTypingAsync();
            // check for attachments
            string dank;
            if (ctx.Message.Attachments.Count < 1) {
                // check msg attachments
                dank = await GetPastAttachment(ctx);
                if (dank == null) {
                    await ctx.RespondAsync("I couldn't find any attachments!");
                    return;
                }
            } else {
                dank = ctx.Message.Attachments[0].Url;
            }
            // download attachment
            using (var client = new WebClient()) {
                Stream stream = await client.OpenReadTaskAsync(new Uri(dank));
                IMagickImage img = new MagickImage(stream);
                img.VirtualPixelMethod = VirtualPixelMethod.Tile;
                img.Resize(512, 512);
                img.Distort(DistortMethod.Perspective, 0, 0, 57, 42, 0, 128, 63, 130, 128, 0, 140, 60, 128, 128, 140, 140);
                img.Format = MagickFormat.Png64;
                var memory = new MemoryStream(img.ToByteArray());
                await ctx.RespondWithFileAsync("wall.png", memory);
                // Dispose of everything used
                await memory.DisposeAsync();
                await stream.DisposeAsync();
                img.Dispose();
            }
        }

        [Command("swirl"), Description("swirls an image. Incredible"), Category(Category.Image)]
        public async Task swirl(CommandContext ctx) {
            // type a thing(tm)
            await ctx.TriggerTypingAsync();
            // check for attachments
            string dank;
            if (ctx.Message.Attachments.Count < 1) {
                // check msg attachments
                dank = await GetPastAttachment(ctx);
                if (dank == null) {
                    await ctx.RespondAsync("I couldn't find any attachments!");
                    return;
                }
            } else {
                dank = ctx.Message.Attachments[0].Url;
            }
            // download attachment
            using (var client = new WebClient()) {
                Stream stream = await client.OpenReadTaskAsync(new Uri(dank));
                IMagickImage img = new MagickImage(stream);
                await Task.Run(() => img.Swirl((double)180));
                img.Format = MagickFormat.Png64;
                var memory = new MemoryStream(img.ToByteArray());
                await ctx.RespondWithFileAsync("swirl.png", memory);
                // Dispose of everything used
                await memory.DisposeAsync();
                await stream.DisposeAsync();
                await Task.Run(() => img.Dispose());
            }
        }

        [Command("jpeg"), Description("make any image into an authentic JPEG"), Category(Category.Image)]
        public async Task jpeg(CommandContext ctx) {
            /// type a thing(tm)
            await ctx.TriggerTypingAsync();
            // check for attachments
            string dank;
            if (ctx.Message.Attachments.Count < 1) {
                // check msg attachments
                dank = await GetPastAttachment(ctx);
                if (dank == null) {
                    await ctx.RespondAsync("I couldn't find any attachments!");
                    return;
                }
            } else {
                dank = ctx.Message.Attachments[0].Url;
            }
            // download attachment
            using (var client = new WebClient()) {
                Stream stream = await client.OpenReadTaskAsync(new Uri(dank));
                IMagickImage img = new MagickImage(stream);
                img.Format = MagickFormat.Jpeg;
                img.Quality = 0;
                img.Quality = -5;
                var memory = new MemoryStream(img.ToByteArray());
                await ctx.RespondWithFileAsync("jpeg.jpg", memory);
                // Dispose of everything used
                await memory.DisposeAsync();
                await stream.DisposeAsync();
                img.Dispose();
            }
           
        }

        [Command("pixel"), Description("reduces an image to 1x1 in size"), Category(Category.Image)]
        public async Task pixel(CommandContext ctx) {
            // type a thing(tm)
            await ctx.TriggerTypingAsync();
            // check for attachments
            string dank;
            if (ctx.Message.Attachments.Count < 1) {
                // check msg attachments
                dank = await GetPastAttachment(ctx);
                if (dank == null) {
                    await ctx.RespondAsync("I couldn't find any attachments!");
                    return;
                }
            } else {
                dank = ctx.Message.Attachments[0].Url;
            }
            // download attachment
            using (var client = new WebClient()) {
                Stream stream = await client.OpenReadTaskAsync(new Uri(dank));
                IMagickImage img = new MagickImage(stream);
                img.Resize(1, 1);
                img.Resize(200, 200);
                img.Format = MagickFormat.Png64;
                var memory = new MemoryStream(img.ToByteArray());
                await ctx.RespondWithFileAsync("jpeg.jpg", memory);
                // Dispose of everything used
                await memory.DisposeAsync();
                await stream.DisposeAsync();
                img.Dispose();
            }
        }

        [Command("explode"), Description("explodes an image"), Category(Category.Image)]
        public async Task explode(CommandContext ctx) {
            // type a thing(tm)
            await ctx.TriggerTypingAsync();
            // check for attachments
            string dank;
            if (ctx.Message.Attachments.Count < 1) {
                // check msg attachments
                dank = await GetPastAttachment(ctx);
                if (dank == null) {
                    await ctx.RespondAsync("I couldn't find any attachments!");
                    return;
                }
            } else {
                dank = ctx.Message.Attachments[0].Url;
            }
            // download attachment
            using (var client = new WebClient()) {
                Stream stream = await client.OpenReadTaskAsync(new Uri(dank));
                IMagickImage img = new MagickImage(stream);
                img.Implode(-2, PixelInterpolateMethod.Bilinear);
                img.Format = MagickFormat.Png64;
                var memory = new MemoryStream(img.ToByteArray());
                await ctx.RespondWithFileAsync("explode.png", memory);
                // Dispose of everything used
                await memory.DisposeAsync();
                await stream.DisposeAsync();
                img.Dispose();
            }
        }

        [Command("implode"), Description("implodes an image"), Category(Category.Image)]
        public async Task implode(CommandContext ctx) {
            // type a thing(tm)
            await ctx.TriggerTypingAsync();
            // check for attachments
            string dank;
            if (ctx.Message.Attachments.Count < 1) {
                // check msg attachments
                dank = await GetPastAttachment(ctx);
                if (dank == null) {
                    await ctx.RespondAsync("I couldn't find any attachments!");
                    return;
                }
            } else {
                dank = ctx.Message.Attachments[0].Url;
            }
            // download attachment
            using (var client = new WebClient()) {
                Stream stream = await client.OpenReadTaskAsync(new Uri(dank));
                IMagickImage img = new MagickImage(stream);
                //IMagickImage img = new MagickImage("Resources/test.png");
                img.Implode(1, PixelInterpolateMethod.Bilinear);
                img.Format = MagickFormat.Png64;
                var memory = new MemoryStream(img.ToByteArray());
                await ctx.RespondWithFileAsync("implode.png", memory);
                // Dispose of everything used
                await memory.DisposeAsync();
                await stream.DisposeAsync();
                img.Dispose();
            }
        }

        [Command("reverse"), Description("busts out everyone's favorite uno card"), Aliases("uno"), Category(Category.Fun)]
        public async Task reverse(CommandContext ctx) {
            await ctx.TriggerTypingAsync();
            // generate random number 1-4
            var rng = Program.rand.Next(6) + 1;
            // open uno card
            var card = new MagickImage($"Resources/Uno/card{rng.ToString()}.png");
            card.Format = MagickFormat.Png64;
            var mem = new MemoryStream(card.ToByteArray());
            card.Dispose();
            await ctx.RespondWithFileAsync("uno.png", mem);
            await mem.DisposeAsync();

        }
    }
}
