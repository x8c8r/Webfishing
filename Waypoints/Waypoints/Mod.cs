using GDWeave;

namespace Waypoints;

public class Mod : IMod {

    public Mod(IModInterface modInterface) {
        modInterface.Logger.Information("Loaded Waypoints! Trans rights!");
        
        modInterface.RegisterScriptMod(new MenuInject());
        
        modInterface.RegisterScriptMod(new PlayerHudPatch());
        modInterface.RegisterScriptMod(new EscMenuPatch());
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
