using System.Text.Json.Serialization;

namespace StreamerMode;

public class Config {
    [JsonInclude] public bool Chatbox = true;
    [JsonInclude] public bool SpeechBubbles = true;
    [JsonInclude] public bool ChalkCanvases = true;
}
