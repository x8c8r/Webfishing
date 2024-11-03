using GDWeave;

namespace StreamerMode;

public class Mod : IMod {
    public static Config Config;

    public Mod(IModInterface modInterface) {
        Mod.Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new ChatboxPatch());
        modInterface.RegisterScriptMod(new SpeechBubblePatch());
        modInterface.RegisterScriptMod(new ChalkCanvasPatch());
        modInterface.Logger.Information("Loaded StreamerMode!");
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
