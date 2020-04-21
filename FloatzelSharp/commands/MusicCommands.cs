using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using FloatzelSharp.kekbot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloatzelSharp.commands {
    class MusicCommands : BaseCommandModule {

        // a lot of this code is ripped from kekbotSharp
        // Thanks jorge!
        private MusicService Music { get; }
        private GuildMusicData GuildMusic { get; set; }

        public MusicCommands(MusicService music) {
            this.Music = music;
        }

        public override async Task BeforeExecutionAsync(CommandContext ctx) {
            var vs = ctx.Member.VoiceState;
            var chn = vs.Channel;
            if (chn == null) {
                await ctx.RespondAsync($"You need to be in a voice channel.");
                throw new CommandCancelledException();
            }

            var mbr = ctx.Guild.CurrentMember?.VoiceState?.Channel;
            if (mbr != null && chn != mbr) {
                await ctx.RespondAsync($"You need to be in the same voice channel.");
                throw new CommandCancelledException();
            }

            if (ctx.Command.CustomAttributes.OfType<RequiresMusicHostAttribute>().Any()) {
                if (!ctx.Channel.PermissionsFor(ctx.Member).HasPermission(Permissions.ManageGuild)) {
                    if (this.GuildMusic.Host != null && this.GuildMusic.Host != ctx.Member) {
                        await ctx.RespondAsync("You aren't the host (Debug Message)");
                        throw new CommandCancelledException();
                    }
                }
            }

            this.GuildMusic = await this.Music.GetOrCreateDataAsync(ctx.Guild);
            this.GuildMusic.CommandChannel ??= ctx.Channel;
            this.GuildMusic.Host ??= ctx.Member;

            await base.BeforeExecutionAsync(ctx);
        }

        async Task Queue(CommandContext ctx, Uri uri) {
            var trackLoad = await this.Music.GetTracksAsync(uri);
            var tracks = trackLoad.Tracks;
            if (trackLoad.LoadResultType == LavalinkLoadResultType.LoadFailed || !tracks.Any()) {
                await ctx.RespondAsync("No tracks were found at specified link.");
                return;
            }

            if (trackLoad.LoadResultType == LavalinkLoadResultType.PlaylistLoaded && trackLoad.PlaylistInfo.SelectedTrack > 0) {
                var index = trackLoad.PlaylistInfo.SelectedTrack;
                tracks = tracks.Skip(index).Concat(tracks.Take(index));
            }

            var trackCount = tracks.Count();
            foreach (var track in tracks)
                this.GuildMusic.Enqueue(new MusicItem(track, ctx.Member));

            var vs = ctx.Member.VoiceState;
            var chn = vs.Channel;
            bool first = await this.GuildMusic.CreatePlayerAsync(chn);
            await this.GuildMusic.PlayAsync();

            if (first) return;
            if (trackCount > 1)
                await ctx.RespondAsync($"Added {trackCount:#,##0} tracks to the queue.");
            else {
                var track = tracks.First();
                await ctx.RespondAsync($"Added {Formatter.InlineCode(track.Title)} by {Formatter.InlineCode(track.Author)} to the queue. (Time before it plays: {Util.PrintTimeSpan(this.GuildMusic.GetTimeBeforeNext())} | {Formatter.Bold($"Queue Position: {this.GuildMusic.Queue.Count}")})");
            }
        }

        [Command("play"), Description("Queues a music track.")]
        async Task Play(CommandContext ctx, [Description("URL to play from.")] Uri URL) {
            await Queue(ctx, URL);
        }

        [Command("stop"), Description("Stops the current music session. (Host Only)"), Aliases("disconnect", "dc", "leave", "fuckoff", "gtfo"), RequiresMusicHost]
        async Task Stop(CommandContext ctx) {
            this.GuildMusic.EmptyQueue();
            await this.GuildMusic.StopAsync();
            await this.GuildMusic.DestroyPlayerAsync();
        }
    }
}
