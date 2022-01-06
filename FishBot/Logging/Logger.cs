using System.Threading.Channels;

namespace FishBot.Logging;

public class Logger : IAsyncDisposable
{
    public static LogLevel ConsoleVerbosity = LogLevel.Info;
    
    private Channel<LogMessage> logChannel = Channel.CreateUnbounded<LogMessage>();
    
    private StreamWriter log;

    private Timer autoFlush;

    public Logger(int timeout = 100)
    {
        log = File.AppendText("log.log");
        autoFlush = new Timer(asyncCallback, true, timeout, timeout);
    }

    private async void asyncCallback(object? o)
    {
        await FlushAsync();
    }

    public async Task LogAsync(LogMessage message)
    {
        await logChannel.Writer.WriteAsync(message);
        //await FlushAsync();
    }

    public async Task LogAsync(string message)
    {
        LogMessage m = new LogMessage(message);
        await LogAsync(m);
    }

    public async Task FlushAsync()
    {
        await foreach (LogMessage m in logChannel.Reader.ReadAllAsync())
        {
            await log.WriteAsync(m.ToString());

            if ((int)ConsoleVerbosity <= (int)m.Severity)//log to console only if verbosity is lower or equal
            {
                await Console.Out.WriteLineAsync(m.ToString());
            }
        }
        await log.FlushAsync();
        await Console.Out.FlushAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await autoFlush.DisposeAsync();
        await FlushAsync();
        await log.DisposeAsync();
    }
}