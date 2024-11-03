using System.Text.Json.Serialization;

namespace YAAM;

public class Config {
    [JsonInclude] public bool Autocast = true;
    [JsonInclude] public bool Autoreel = true;
    [JsonInclude] public bool Automash = true;
}
