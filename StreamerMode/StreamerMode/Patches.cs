using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace StreamerMode;

public class ChatboxPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/playerhud.gdc" && !Mod.Config.Chatbox;

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var functionWaiter = new FunctionWaiter("_ready");

        foreach (var token in tokens)
        {
            if (functionWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.Dollar);
                yield return new IdentifierToken("main");
                yield return new Token(TokenType.OpDiv);
                yield return new IdentifierToken("in_game");
                yield return new Token(TokenType.OpDiv);
                yield return new IdentifierToken("gamechat");         
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("visible");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                
                yield return new Token(TokenType.Newline, 1);
            }
            else
            {
                yield return token;
            }
        }
        
    }
}

public class SpeechBubblePatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/SpeechBubble/speech_bubble.gdc" && !Mod.Config.SpeechBubbles;

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var functionWaiter = new FunctionWaiter("_ready");

        foreach (var token in tokens)
        {
            if (functionWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);
                
                yield return new IdentifierToken("visible");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                
                yield return new Token(TokenType.Newline, 1);
            }
            else
            {
                yield return token;
            }
        }
    }
}

public class ChalkCanvasPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/ChalkCanvas/chalk_canvas.gdc" && !Mod.Config.ChalkCanvases;

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var functionWaiter = new FunctionWaiter("_ready");

        foreach (var token in tokens)
        {
            if (functionWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);
                
                yield return new IdentifierToken("visible");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                
                yield return new Token(TokenType.Newline, 1);
            }
            else
            {
                yield return token;
            }
        }
    }
}