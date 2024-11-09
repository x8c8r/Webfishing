using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace YAAM;

public class PlayerData : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/playerdata.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var baitUseWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken {Name:"bait_inv"},
            t => t.Type is TokenType.BracketOpen,
            t => t is IdentifierToken {Name:"bait_id"},
            t => t.Type is TokenType.BracketClose,
            t => t.Type is TokenType.OpLessEqual,
            t => t is ConstantToken { Value: IntVariant { Value: 0}},
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline,
        ]);
        
        
        var newlineConsumer = new TokenConsumer(t => t.Type is TokenType.Newline);

        foreach (var token in tokens)
        {
            if (newlineConsumer.Check(token)) continue;

            if (newlineConsumer.Ready)
            {
                yield return token;
                newlineConsumer.Reset();
            }
            
            if (baitUseWaiter.Check(token))
            {
                // if YAAM.config.BaitAutoRefill and PlayerData.bait_inv[PlayerData.bait_selected] == 0 and PlayerData.bait_selected != "":
                // PlayerData._refill_bait(bait_add)
                // PlayerData.money -= PlayerData.BAIT_DATA[PlayerData.bait_selected].cost
                
                yield return new Token(TokenType.Newline, 2);

                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.Dollar);
                yield return new ConstantToken(new StringVariant("/root/YAAM"));
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("BaitAutoRefill");

                yield return new Token(TokenType.OpAnd);
                
                yield return new IdentifierToken("money");
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken("BAIT_DATA");
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("bait_id");
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("cost");
                yield return new Token(TokenType.OpGreaterEqual);
                yield return new ConstantToken(new IntVariant(0));

                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 3);
                
                yield return new IdentifierToken("_refill_bait");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("bait_id");
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 3);
                
                yield return new IdentifierToken("money");
                yield return new Token(TokenType.OpAssignSub);
                yield return new IdentifierToken("BAIT_DATA");
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("bait_id");
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("cost");
                
                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);
                
            }
            else yield return token;
        }
    }
}