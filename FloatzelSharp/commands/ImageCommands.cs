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
    }
}
