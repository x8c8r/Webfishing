using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace Waypoints;

public class MenuInject : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/Esc Menu/esc_menu.gdc" || path == "res://Scenes/HUD/playerhud.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var extendsWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrExtends,
            t => t.Type is TokenType.Newline
        ], allowPartialMatch: true);
        
        foreach (var token in tokens)
        {
            if (extendsWaiter.Check(token))
            {
                yield return token;

                yield return new Token(TokenType.Newline);

                yield return new Token(TokenType.PrOnready);
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("Waypoints");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/Waypoints"));
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline);
            }
            else
            {
                yield return token;
            }
        }
    }
}