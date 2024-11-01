using System.Diagnostics;
using System.Runtime.CompilerServices;
using Pastel;

namespace Miyu.Core {
    public enum LogSeverity {
        Critical,
        Debug,
        Error,
        Info,
        Verbose,
        Warning,
    }

    // public class LogEntry {
    //     public object Sender = null!;
    //     public string Message = "EMPTY MESSAGE";
    //     public LogSeverity Severity = LogSeverity.Debug;
    //     public Exception? Exception = null;
    //     public LogEntry(object sender, string message, LogSeverity severity, Exception? exception = null) {
    //         sender = Sender!;
    //         message = Message;
    //         severity = Severity;
    //         exception = Exception;
    //     }
    // }

    public class Logger {
        private static readonly Dictionary<LogSeverity, string> ColorTable = new() {
            {LogSeverity.Critical, "#ea00ff"},
            {LogSeverity.Debug, "#fbff00"},
            {LogSeverity.Error, "#ff3434"},
            {LogSeverity.Info, "#4560f7"},
            {LogSeverity.Verbose, "#ff9729"},
            {LogSeverity.Warning, "#ff3434"},
        };
        // private static Stack<LogEntry> logStack = new();
        // public static void New(object sender, string message, LogSeverity severity, Exception? exception = null) {
        //     LogEntry log = new(sender, message, severity, exception);
        //     logStack.Push(log);
        // }
        // private void Print() {

        // }

        // This code looks like absolute ASS with 431884193 different variables, but it works

        public static void New(
            string message,
            LogSeverity severity,
            Exception? exception = null,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string callerPath = "",
            [CallerLineNumber] int callerLineNumber = -1
        ) {
            string? currentTime = DateTime.Now.ToString("h:mm:ss");
            string callerClassPath = new StackTrace().GetFrame(1)?.GetMethod()?.DeclaringType?.FullName ?? "EMPTY_CLASS";
            callerClassPath = callerClassPath[..callerClassPath.IndexOf('+')];
            string callerMessage = callerPath[callerPath.IndexOf("miyu\\")..].Replace("miyu\\", "").Replace("\\", " / ") + $" [{callerClassPath}.{caller}() : {callerLineNumber}]";
            var sender = callerMessage.Trim();
            DotNetEnv.Env.Load();
            int loggerMaxSenderChars = DotNetEnv.Env.GetInt("loggerMaxSenderChars", 100);
            if (sender.ToString().Length > loggerMaxSenderChars) {
                Environment.SetEnvironmentVariable("loggerMaxSenderChars", sender.ToString().Length.ToString());
                loggerMaxSenderChars = int.Parse(Environment.GetEnvironmentVariable("loggerMaxSenderChars")!);
            }
            var senderSpaces = new string(' ', loggerMaxSenderChars + 1 - (sender.ToString() ?? "").Length);
            var severitySpaces = new string(' ', 8 - severity.ToString().Length);
            var severityColored = (severity.ToString() + severitySpaces).Pastel(ColorTable[severity]);

            string logString = $"{currentTime.Pastel("#5c5c5c")} " +
                              $"{"「".Pastel("#cdcdcd")}{sender.ToString().Pastel("#13bf61")}{senderSpaces}: {severityColored}" +
                              $"{"」".Pastel("#cdcdcd")}⇢ {message.Pastel(ColorTable[severity])}";

            if (exception != null) {
                logString += $"\n{currentTime.Pastel("#5c5c5c")} " +
                              $"{"「".Pastel("#cdcdcd")}{sender.ToString().Pastel("#13bf61")}{senderSpaces}: {severityColored}" +
                              $"{"」".Pastel("#cdcdcd")}↳       {exception!.ToString().Pastel(ColorTable[severity])}";
            }

            Console.WriteLine(logString);
        }
    }
}