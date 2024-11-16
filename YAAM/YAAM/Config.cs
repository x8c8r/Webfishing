using System.Text.Json.Serialization;

namespace YAAM;

public class Config {
    [JsonInclude] public bool Autocast = true;
    [JsonInclude] public bool Autoreel = true;
    [JsonInclude] public bool Automash = true;
    [JsonInclude] public double CastDistance = 1.5;

    [JsonInclude] public bool AutocastRequiresBait = true;
    [JsonInclude] public bool BaitAutoRefill = true;

    [JsonInclude] public Dictionary<string, bool> CatchQualities = new()
    {
        {"Normal", true},
        {"Shining", true},
        {"Glistening", true},
        {"Opulent", true},
        {"Radiant", true},
        {"Alpha", true},
    };
}