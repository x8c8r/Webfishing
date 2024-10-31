using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace QuickPlay;

public class MainMenuPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Menus/Main Menu/main_menu.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var readyMatch = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "_ready"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline
        ]);

        // thanks to jade (https://github.com/puppy-girl) for the help
        foreach (var token in tokens)
        {
            if (readyMatch.Check(token))
            {
                yield return token;
                
                // $VBoxContainer/play_online.visible = true
                yield return new Token(TokenType.Dollar);
                yield return new ConstantToken(new StringVariant("VBoxContainer/play_online"));
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("visible");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Newline);

                yield return token;
            }
            else
            {
                yield return token;
            }
        }
    }
}