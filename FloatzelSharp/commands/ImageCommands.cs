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

namespace FloatzelSharp.commands {
    class ImageCommands : BaseCommandModule {

        [Command("imagetest"), Description("this command exists to make sure "), Category(Category.Test)]
        public async Task dank(CommandContext ctx) {
            await ctx.TriggerTypingAsync();

            WebClient client = new WebClient();
            Stream stream = await client.OpenReadTaskAsync(new Uri(ctx.User.AvatarUrl));
            IMagickImage ava = new MagickImage(stream);
            Stream streamm = new MemoryStream();
            IMagickImage test2 = new MagickImage(MagickColor.FromRgba(255, 0, 0, 255), ava.Width, ava.Height);
            test2.Format = MagickFormat.Png64;
            test2.Composite(ava, 50, 50, CompositeOperator.SrcOver);
            await ctx.RespondWithFileAsync("test.png", new MemoryStream(test2.ToByteArray()), "test");
        }

        [Command("wall"), Description("make a wall from an attachment"), Category(Category.Image)]
        public async Task wall(CommandContext ctx) {
            // type a thing(tm)
            await ctx.TriggerTypingAsync();
            // check for attachments
            if(ctx.Message.Attachments.Count < 0) {
                await ctx.RespondAsync("You did not give me an attachment you fool!");
                return;
            }
            // download attachment
            WebClient client = new WebClient();
            var dank = ctx.Message.Attachments;
            Stream stream = await client.OpenReadTaskAsync(new Uri(dank[0].Url));
            IMagickImage img = new MagickImage(stream);
            img.VirtualPixelMethod = VirtualPixelMethod.Tile;
            img.Resize(512, 512);
            img.Distort(DistortMethod.Perspective,  0, 0, 57, 42, 0, 128, 63, 130, 128, 0, 140, 60, 128, 128, 140, 140);
            img.Format = MagickFormat.Png64;
            await ctx.RespondWithFileAsync("wall.png", new MemoryStream(img.ToByteArray()));
        }

        [Command("swirl"), Description("swirls an image. Incredible"), Category(Category.Image)]
        public async Task swirl(CommandContext ctx) {
            // TODO: check message history
            await ctx.TriggerTypingAsync();
            // check for attachments
            if (ctx.Message.Attachments.Count < 0) {
                await ctx.RespondAsync("You did not give me an attachment you fool!");
                return;
            }
            // download attachment
            WebClient client = new WebClient();
            var dank = ctx.Message.Attachments;
            Stream stream = await client.OpenReadTaskAsync(new Uri(dank[0].Url));
            IMagickImage img = new MagickImage(stream);
            img.Swirl((double)180);
            img.Format = MagickFormat.Png64;
            await ctx.RespondWithFileAsync("swirl.png", new MemoryStream(img.ToByteArray()));
        }

        [Command("jpeg"), Description("make any image into an authentic JPEG"), Category(Category.Image)]
        public async Task jpeg(CommandContext ctx) {
            // TODO: check message history
            await ctx.TriggerTypingAsync();
            // check for attachments
            if (ctx.Message.Attachments.Count < 0) {
                await ctx.RespondAsync("You did not give me an attachment you fool!");
                return;
            }
            // download attachment
            WebClient client = new WebClient();
            var dank = ctx.Message.Attachments;
            Stream stream = await client.OpenReadTaskAsync(new Uri(dank[0].Url));
            IMagickImage img = new MagickImage(stream);
            var h = dank[0].Height;
            var w = dank[0].Width;
            img.Format = MagickFormat.Jpeg;
            img.Quality = 0;
            img.Resize(w / 2 / 2, h / 2 / 2);
            img.Format = MagickFormat.Jpeg;
            img.Quality = 0;
            img.Resize(w, h);
            img.Quality = 0;
            await ctx.RespondWithFileAsync("jpeg.jpg", new MemoryStream(img.ToByteArray()));
        }

        [Command("pixel"), Description("reduces an image to 1x1 in size"), Category(Category.Image)]
        public async Task pixel(CommandContext ctx) {
            // TODO: check message history
            await ctx.TriggerTypingAsync();
            // check for attachments
            if (ctx.Message.Attachments.Count < 0) {
                await ctx.RespondAsync("You did not give me an attachment you fool!");
                return;
            }
            // download attachment
            WebClient client = new WebClient();
            var dank = ctx.Message.Attachments;
            Stream stream = await client.OpenReadTaskAsync(new Uri(dank[0].Url));
            IMagickImage img = new MagickImage(stream);
            img.Resize(1, 1);
            img.Resize(200, 200);
            img.Format = MagickFormat.Png64;
            await ctx.RespondWithFileAsync("jpeg.jpg", new MemoryStream(img.ToByteArray()));
        }

        [Command("explode"), Description("explodes an image"), Category(Category.Image)]
        public async Task explode(CommandContext ctx) {
            // TODO: check message history
            await ctx.TriggerTypingAsync();
            // check for attachments
            if (ctx.Message.Attachments.Count < 0) {
                await ctx.RespondAsync("You did not give me an attachment you fool!");
                return;
            }
            // download attachment
            WebClient client = new WebClient();
            var dank = ctx.Message.Attachments;
            Stream stream = await client.OpenReadTaskAsync(new Uri(dank[0].Url));
            IMagickImage img = new MagickImage(stream);
            img.Implode(-2, PixelInterpolateMethod.Bilinear);
            img.Format = MagickFormat.Png64;
            await ctx.RespondWithFileAsync("explode.png", new MemoryStream(img.ToByteArray()));
        }

        [Command("implode"), Description("implodes an image"), Category(Category.Image)]
        public async Task implode(CommandContext ctx) {
            // TODO: check message history
            await ctx.TriggerTypingAsync();
            // check for attachments
            if (ctx.Message.Attachments.Count < 0) {
                await ctx.RespondAsync("You did not give me an attachment you fool!");
                return;
            }
            // download attachment
            WebClient client = new WebClient();
            var dank = ctx.Message.Attachments;
            Stream stream = await client.OpenReadTaskAsync(new Uri(dank[0].Url));
            IMagickImage img = new MagickImage(stream);
            img.Implode(1, PixelInterpolateMethod.Bilinear);
            img.Format = MagickFormat.Png64;
            await ctx.RespondWithFileAsync("implode.png", new MemoryStream(img.ToByteArray()));
        }
    }
}
