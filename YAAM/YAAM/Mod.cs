using GDWeave;

namespace YAAM;

public class Mod : IMod {
    public static Config Config;

    public Mod(IModInterface modInterface) {
        Mod.Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new FishingRodPatch());
        modInterface.RegisterScriptMod(new ControllerProcessPatch());
        modInterface.RegisterScriptMod(new MinigamePatch());
        modInterface.RegisterScriptMod(new InputPatch());
        modInterface.Logger.Information("Loaded YAAM!");
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
