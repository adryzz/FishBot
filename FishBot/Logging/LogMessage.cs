using NodaTime;

namespace FishBot.Logging;

public class LogMessage
{
    public Instant LogTime { get; }

    public LogType Type { get; set; }

    public LogLevel Severity { get; set; }

    public string Message { get; set; }

    public bool Colored { get; set; }

    public LogMessage(string text, LogType type = LogType.Runtime, LogLevel severity = LogLevel.Info, bool colored = true)
    {
        //get time and round it up to the nearest second
        Instant now = Instant.FromUnixTimeSeconds(SystemClock.Instance.GetCurrentInstant().ToUnixTimeSeconds());
        Message = text;
        LogTime = now;
        Type = type;
        Severity = severity;
        Colored = colored;
    }

    public override string ToString()
    {
        if (Colored)
        {
            return $"{Severity.GetColor()}[{Severity.ToString().ToUpper()}]{Reset} | {Type.GetColor()}[{Type.ToString().ToUpper()}]{Reset} | {Grey}{LogTime}{Reset} | {Message}\n";
        }
        
        return $"[{Severity.ToString().ToUpper()}] | [{Type.ToString().ToUpper()}] | {LogTime} | {Message}\n";
    }

    private const string Reset = "\u001b[0m";
    private const string Grey = "\u001b[0m";
}