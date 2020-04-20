﻿using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FloatzelSharp.kekbot {
    public sealed class LavalinkService {
        public LavalinkNodeConnection LavalinkNode { get; private set; }

        public LavalinkService(DiscordClient Client) {
            Client.Ready += Ready;
        }

        private async Task Ready(ReadyEventArgs e) {
            if (this.LavalinkNode != null) return;

            var lava = e.Client.GetLavalink();
            this.LavalinkNode = await lava.ConnectAsync(new LavalinkConfiguration {
                Password = "youshallnotpass",

                SocketEndpoint = new ConnectionEndpoint("localhost", 2333),
                RestEndpoint = new ConnectionEndpoint("localhost", 2333)
            });
        }
    }
}
