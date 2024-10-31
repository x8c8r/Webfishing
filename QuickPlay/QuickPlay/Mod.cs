using GDWeave;

namespace QuickPlay;

public class Mod : IMod {

    public Mod(IModInterface modInterface) {
        modInterface.RegisterScriptMod(new MainMenuPatch());
        modInterface.Logger.Information("Loaded QuickPlay!");
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
