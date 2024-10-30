using GDWeave;

namespace FreeFreecam;

public class Mod : IMod {
    public Mod(IModInterface modInterface) {
        modInterface.RegisterScriptMod(new FreecamPatch());
        modInterface.Logger.Information("FreeFreecam is loaded!");
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
