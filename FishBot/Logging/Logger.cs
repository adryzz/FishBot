using System.Threading.Channels;

namespace FishBot.Logging;

public class Logger : IAsyncDisposable
{
    public static LogLevel ConsoleVerbosity = LogLevel.Info;
    
    private Channel<LogMessage> logChannel = Channel.CreateUnbounded<LogMessage>();
    
    private StreamWriter log;
    
    public Logger()
    {
        log = File.AppendText("log.log");
    }
    
    public async Task LogAsync(LogMessage message)
    {
        await logChannel.Writer.WriteAsync(message);
        await Task.Delay(50);
        await FlushAsync();
    }

    public async Task Log(string message)
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
    }

    public async ValueTask DisposeAsync()
    {
        await FlushAsync();
        await log.DisposeAsync();
    }
}