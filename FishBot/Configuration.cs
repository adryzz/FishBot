using System.Text.Json;
using Discord;

namespace FishBot;

[Serializable]
public class Configuration
{
    public string Token { get; set; } = "";
    public string BotOwner { get; set; } = "";
    public string CmdPrefix { get; set; } = ".";
    public string Game { get; set; } = "";
    public string Url { get; set; } = "";
    public Color Color { get; set; } = Color.Blue;
    public UserStatus Status { get; set; } = UserStatus.Online;

    public JsonSerializerOptions ConfigOptions { get; set; } = new JsonSerializerOptions { WriteIndented = true };

    public static Configuration FromFile(string fileName)
    {
        return JsonSerializer.Deserialize<Configuration>(File.ReadAllText(fileName));
    }

    public void Save(string fileName)
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(this, ConfigOptions)); 
    }
}