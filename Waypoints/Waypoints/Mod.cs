using GDWeave;

namespace Waypoints;

public class Mod : IMod {

    public Mod(IModInterface modInterface) {
        modInterface.Logger.Information("Loaded Waypoints! Trans rights!");
        
        // modInterface.RegisterScriptMod(new EscMenuInject());
        modInterface.RegisterScriptMod(new EscMenuPatch());
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
