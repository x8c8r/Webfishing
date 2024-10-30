using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace FreeFreecam;

public class FreecamPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var freecamInputWaiter = new FunctionWaiter("_freecam_input", true);
        
        var maxDistWaiter = new MultiTokenWaiter([
        t => t.Type is TokenType.PrVar,
        t => t is IdentifierToken {Name: "max_dist"},
        t => t.Type is TokenType.OpAssign,
        t => t is ConstantToken { Value: RealVariant {Value: 15.0 }}
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
}