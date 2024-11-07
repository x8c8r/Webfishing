using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace FreeFreecam;

public class FreecamPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var maxDistWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrVar,
            t => t is IdentifierToken { Name: "max_dist" },
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken { Value: RealVariant }
        ]);

        foreach (var token in tokens)
        {
            if (maxDistWaiter.Check(token))
            {
                yield return new ConstantToken(new RealVariant(99999.9));
            }
            else
            {
                yield return token;
            }
        }
    }
    //     var newlineConsumer = new TokenConsumer(t => t.Type is TokenType.Newline);
    //     var clampWaiter = new MultiTokenWaiter([
    //         t => t is IdentifierToken { Name: "freecam_anchor" },
    //         t => t.Type is TokenType.Period,
    //         t => t is IdentifierToken { Name: "global_transform" },
    //         t => t.Type is TokenType.Period,
    //         t => t is IdentifierToken { Name: "origin" },
    //
    //         t => t is IdentifierToken { Name: "build_dir" },
    //         t => t.Type is TokenType.Period,
    //         t => t is IdentifierToken { Name: "speed" },
    //         t => t.Type is TokenType.Period,
    //         t => t is IdentifierToken { Name: "delta" },
    //
    //         t => t.Type is TokenType.Newline,
    //         t => t.Type is TokenType.Newline,
    //     ]);
    //     
    //     int linesConsumed = 0;
    //     bool consume = false;
    //     foreach (var token in tokens)
    //     {
    //
    //         if (newlineConsumer.Check(token)) continue;
    //
    //         if (newlineConsumer.Ready)
    //         {
    //             yield return token;
    //             newlineConsumer.Reset();
    //         }
    //
    //         if (clampWaiter.Check(token))
    //         {
    //             consume = true;
    //         }
    //         else if (linesConsumed < 3 && consume)
    //         {
    //             newlineConsumer.SetReady();
    //             linesConsumed++;
    //         }
    //         else yield return token;
    //     }
    // }
}