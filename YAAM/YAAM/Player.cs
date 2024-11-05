using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace YAAM;
public class FishingRodPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var functionWaiter = new FunctionWaiter("_cast_fishing_rod");
        var cfrWaiter1 = new MultiTokenWaiter([
            t => t is IdentifierToken { Name: "_enter_animation" },
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken { Value: StringVariant { Value: "rod_wind" } },
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken { Value: BoolVariant { Value: true } },
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken { Value: BoolVariant { Value: false } },
            t => t.Type is TokenType.Comma,
            t => t.Type is TokenType.OpSub,
            t => t is ConstantToken { Value: IntVariant { Value: 1 } },
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken { Value: BoolVariant { Value: false } },
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Newline
        ], true, true);

        var crfWaiter2 = new MultiTokenWaiter([
            t => t is IdentifierToken { Name: "_sync_sfx" },
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken { Value: StringVariant { Value: "woosh" } },
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken { Value: NilVariant },
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken { Value: RealVariant { Value: 1.0 } },
            t => t.Type is TokenType.Comma,
            t => t is ConstantToken { Value: RealVariant { Value: 0.4 } },
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Newline
        ], true, true);

        var newlineConsumer = new TokenConsumer(t => t.Type is TokenType.Newline);

        foreach (var token in tokens)
        {
            if (functionWaiter.Check(token))
            {
                cfrWaiter1.SetReady();
                crfWaiter2.SetReady();
            }

            if (newlineConsumer.Check(token)) continue;

            if (newlineConsumer.Ready)
            {
                yield return token;
                newlineConsumer.Reset();
            }

            if (cfrWaiter1.Check(token))
            {
                yield return token;
                
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Autocast");
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.PrYield);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.Self);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_primary_release"));
                yield return new Token(TokenType.ParenthesisClose);
                
                newlineConsumer.SetReady();
            }
            else if (crfWaiter2.Check(token))
            {
                yield return token;
                
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("strength");

                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Autocast");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("strength");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("CastDistance");

                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("strength");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("clamp");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("primary_hold_timer");
                yield return new Token(TokenType.OpMul);
                yield return new ConstantToken(new RealVariant(0.06));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new RealVariant(1.5));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new RealVariant(9.0));
                yield return new Token(TokenType.ParenthesisClose);
                
                yield return new Token(TokenType.Newline, 1);

                newlineConsumer.SetReady();
            }
            else
            {
                yield return token;
            }
        }
    }
}

public class ControllerProcessPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var newlineConsumer = new TokenConsumer(t => t.Type is TokenType.Newline);
        var functionWaiter = new FunctionWaiter("_controlled_process");

        var bobberWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken { Name: "fish_detect" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "translation" },
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name: "z" },
            t => t.Type is TokenType.OpAssign,
            t => t.Type is TokenType.OpSub,
            t => t is IdentifierToken { Name: "rod_cast_dist" },
            t => t.Type is TokenType.Newline
        ]);

        foreach (var token in tokens)
        {
            if (newlineConsumer.Check(token)) continue;

            if (newlineConsumer.Ready)
            {
                yield return token;
                newlineConsumer.Reset();
            }
            
            if (functionWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("state");
                yield return new Token(TokenType.OpEqual);
                yield return new IdentifierToken("STATES");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("DEFAULT");

                yield return new Token(TokenType.OpAnd);

                yield return new IdentifierToken("held_item");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("id");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("begins_with");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("fishing_rod"));
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.OpAnd);
                
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Autocast");
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 2);

                yield return new IdentifierToken("_cast_fishing_rod");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("state");
                yield return new Token(TokenType.OpEqual);
                yield return new IdentifierToken("STATES");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("SHOWCASE");

                yield return new Token(TokenType.OpAnd);
                
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Autocast");
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 2);

                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("hud");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("dialogue");
                yield return new Token(TokenType.Colon);

                yield return new IdentifierToken("hud");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("dialogue");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("emit_signal");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("_finished"));
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 2);

                yield return new IdentifierToken("_exit_showcase");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 1);
            }
            else if (bobberWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);
                
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Autocast");
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("bobber_preview");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("translation");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("z");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken("YAAM");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("CastDistance");

                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new IdentifierToken("bobber_preview");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("translation");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("z");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken("clamp");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("primary_hold_timer");
                yield return new Token(TokenType.OpMul);
                yield return new ConstantToken(new RealVariant(0.06));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new RealVariant(1.5));
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new RealVariant(9.0));
                yield return new Token(TokenType.ParenthesisClose);
                
                yield return new Token(TokenType.Newline, 1);
                
                newlineConsumer.SetReady();
            }
            else
            {
                yield return token;
            }
        }
    }
}