using GDWeave;
using GDWeave.Modding;
using GDWeave.Godot;
using GDWeave.Godot.Variants;

namespace YAAM;

public class Mod : IMod {
    public static Config Config;

    public Mod(IModInterface modInterface) {
        Mod.Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new InjectConfig());
        modInterface.RegisterScriptMod(new InjectMenu());
        
        modInterface.RegisterScriptMod(new FishingRodPatch()); 
        modInterface.RegisterScriptMod(new ControllerProcessPatch());
        modInterface.RegisterScriptMod(new MinigamePatch());
        modInterface.RegisterScriptMod(new PlayerData());
        
        modInterface.RegisterScriptMod(new MyOCWouldDoThis());
        
        modInterface.Logger.Information("Loaded YAAM!");
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}

public class InjectConfig : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc" || path == "res://Scenes/Minigames/Fishing3/fishing3.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var extendsWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrExtends,
            t => t.Type is TokenType.Newline
        ], allowPartialMatch: true);

        var readyWaiter = new FunctionWaiter("_ready");

        foreach (var token in tokens)
        {
            if (extendsWaiter.Check(token))
            {
                yield return token;
                
                yield return new Token(TokenType.Newline);

                yield return new Token(TokenType.PrOnready);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.Dollar);
                yield return new ConstantToken(new StringVariant("/root/YAAM"));
                
                yield return new Token(TokenType.Newline);
            }
            else
            {
                yield return token;
            }
        }
    }
}

public class InjectMenu : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var extendsWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrExtends,
            t => t.Type is TokenType.Newline
        ], allowPartialMatch: true);
        var readyWaiter = new FunctionWaiter("_ready");
        
        foreach (var token in tokens)
        {
            if (extendsWaiter.Check(token))
            {
                yield return token;
                
                yield return new Token(TokenType.Newline);
                
                yield return new Token(TokenType.PrOnready);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.Dollar);
                yield return new ConstantToken(new StringVariant("/root/YAAM"));
                
                yield return new Token(TokenType.Newline);
            }
            else if (readyWaiter.Check(token))
            {
                yield return token;

                yield return new Token(TokenType.Newline, 1);

                //$main/in_game.add_child(quick_menu.instance())
                yield return new Token(TokenType.Dollar);
                yield return new IdentifierToken("main");
                yield return new Token(TokenType.OpDiv);
                yield return new IdentifierToken("in_game");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("add_child");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("quick_menu");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("instance");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                
                yield return new Token(TokenType.Newline, 1);
            }
            else yield return token;
        }
    }
}