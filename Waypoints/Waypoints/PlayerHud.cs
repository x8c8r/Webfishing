using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace Waypoints;

public class PlayerHudPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var hideHudWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken { Name: "Input" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "is_action_just_pressed" },
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken { Value: StringVariant { Value: "hide_hud" } },
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon
        ], allowPartialMatch:true);

        var changeMenuWaiter = new FunctionWaiter("_change_menu");

        foreach (var token in tokens)
        {
            if (hideHudWaiter.Check(token))
            {
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("Waypoints");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("menu_opened");

                yield return token;
            }
            else if (changeMenuWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("Waypoints");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("menu_opened");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.CfReturn);
                
                yield return new Token(TokenType.Newline, 1);
            }
            else yield return token;
        }
    }
}