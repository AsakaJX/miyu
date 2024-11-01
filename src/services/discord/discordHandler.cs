using Discord;
using Discord.WebSocket;
using Miyu.Core;

namespace Miyu.Discord {
    public class Init {
        private static DiscordSocketClient? _client;

        public async Task Run() {
            DotNetEnv.Env.Load();
            Logger.New("meow", Core.LogSeverity.Info);
            _client = new DiscordSocketClient();
            _client.Log += Log;

            var token = Environment.GetEnvironmentVariable("discordToken");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static async Task Log(LogMessage message) {
            Logger.New(message.Message, Core.LogSeverity.Info);
        }
    }
}