using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace YAAM;

public class FishingRodPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc" && Mod.Config.Autocast;

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
                newlineConsumer.SetReady();
            }
            else if (crfWaiter2.Check(token))
            {
                yield return token;

                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("strength");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new RealVariant(1.2));

                // yield return new Token(TokenType.Newline, 1);

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
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc" && Mod.Config.Autocast;

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var functionWaiter = new FunctionWaiter("_controlled_process");

        foreach (var token in tokens)
        {
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
            else
            {
                yield return token;
            }
        }
    }
}

public class InputPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc" && Mod.Config.Autocast;

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var functionWaiter = new FunctionWaiter("_get_input");

        foreach (var token in tokens)
        {
            if (functionWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_just_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("menu_open"));
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("held_item");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("id");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("begins_with");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("fishing_rod"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 2);

                yield return new IdentifierToken("_update_held_item");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("FALLBACK_ITEM");
                yield return new Token(TokenType.ParenthesisClose);

                yield return new Token(TokenType.Newline, 1);
            }
            else yield return token;
        }
    }
}