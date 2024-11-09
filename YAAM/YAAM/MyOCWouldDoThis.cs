using GDWeave;
using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;
using Serilog;
using Serilog.Core;

namespace YAAM;

public class MyOCWouldDoThis : IScriptMod // peak of feeding into my ego :3
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player_label.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var postContributorWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken { Name: "Network" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "KNOWN_CONTRIBUTORS" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "has" },
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken { Name: "player_id" },
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t is IdentifierToken { Name: "_name" },
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken { Value: StringVariant },
            t => t.Type is TokenType.OpAdd,
            t => t is IdentifierToken { Name: "_name" },
            t => t.Type is TokenType.Newline
        ], allowPartialMatch:true);

        foreach (var token in tokens)
        {
            if (postContributorWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);
                
                yield return new Token(TokenType.BuiltInFunc, (uint)BuiltinFunction.TextPrint);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("player_id");
                yield return new Token(TokenType.ParenthesisClose);
                
                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.BuiltInFunc, (uint)BuiltinFunction.TextStr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("player_id");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpEqual);
                yield return new ConstantToken(new StringVariant("76561198420614020"));
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("_name");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("[color=#DB5EFF][YAAM CREATOR][/color]\n"));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("_name");
                
                yield return new Token(TokenType.Newline, 1);
            }
            else yield return token;
        }
    }
}