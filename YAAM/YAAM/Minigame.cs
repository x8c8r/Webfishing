using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace YAAM;

public class MinigamePatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Minigames/Fishing3/fishing3.gdc";
    
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var functionWaiter = new FunctionWaiter("_physics_process");
        
        var newlineConsumer = new TokenConsumer(t => t.Type is TokenType.Newline);

        var mashWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken { Name: "active" },
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline
        ], true, true);

        var reelWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken { Name: "_on_Button_pressed" },
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Newline,
        ], true, true);

        foreach (var token in tokens)
        {
            if (functionWaiter.Check(token))
            {
                if (Mod.Config.Automash) mashWaiter.SetReady();
                if (Mod.Config.Autoreel) reelWaiter.SetReady();
            }

            if (newlineConsumer.Check(token)) continue;
            
            if (newlineConsumer.Ready)
            {
                yield return token;
                newlineConsumer.Reset();
            }

            if (mashWaiter.Check(token))
            {
                yield return token;

                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Automash");

                yield return new Token(TokenType.OpOr);
                
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_just_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("primary_action"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("reeling");
                yield return new Token(TokenType.ParenthesisClose);
                
                yield return new Token(TokenType.Colon);
                
                newlineConsumer.SetReady();
            }
            else if (reelWaiter.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline, 1);
            
                yield return new Token(TokenType.CfIf);
                
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Autoreel");

                yield return new Token(TokenType.OpOr);
                
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("primary_action"));
                yield return new Token(TokenType.ParenthesisClose);
                
                yield return new Token(TokenType.Colon);
            
                newlineConsumer.SetReady();
            }
            else yield return token;
        }
    }
}