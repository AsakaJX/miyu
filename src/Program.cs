namespace Miyu.Core {
    public class Init {
        // Redirect programm entry point
        private static async Task Main() => await Run();

        private static readonly string[] envDefaultConfig = new string[2] { "loggerMaxSenderChars=50", "discordToken=\"ENTER_DISCORD_TOKEN_HERE\"" };

        public static async Task Run() {
            if (!File.Exists(".env")) {
                File.Create(".env").Close();
                File.AppendAllLines(".env", envDefaultConfig);
            }

            Logger.New("Miyu is starting!", LogSeverity.Info);

            var DiscordClient = new Discord.Init();
            await DiscordClient.Run();
        }
    }
}