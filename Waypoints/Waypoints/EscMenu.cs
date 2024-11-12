using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace Waypoints;

public class EscMenuPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/HUD/Esc Menu/esc_menu.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        var spawnPressedWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken { Name:"_on_spawn_pressed"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline,
            
            t => t is IdentifierToken { Name:"PlayerData"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken { Name:"emit_signal"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken { Value: StringVariant },
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Newline
        ]);

        var readyWaiter = new FunctionWaiter("_ready");

        foreach (var token in tokens)
        {
            if (spawnPressedWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline);
                
                yield return new Token(TokenType.PrFunction);
                yield return new IdentifierToken("_on_waypoints_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                
                yield return new Token(TokenType.Newline, 1);

                yield return new Token(TokenType.Dollar);
                yield return new IdentifierToken("waypoints_menu");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_open");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
            }
            else if (readyWaiter.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);
                
                // yield return new Token(TokenType.PrVar);
                // yield return new IdentifierToken("Waypoints");
                // yield return new Token(TokenType.OpAssign);
                // yield return new IdentifierToken("get_node");
                // yield return new Token(TokenType.ParenthesisOpen);
                // yield return new ConstantToken(new StringVariant("/root/Waypoints"));
                // yield return new Token(TokenType.ParenthesisClose);
                //
                // yield return new Token(TokenType.Newline, 1);
                
                // var waypoint_btn = Waypoints.MENU_BUTTON.instance()
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("waypoint_btn");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("Waypoints");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("MENU_BUTTON");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("instance");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                // buttonlist.add_child(waypoint_btn)
                yield return new IdentifierToken("buttonlist");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("add_child");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("waypoint_btn");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                
                // buttonlist.move_child(waypoint_btn, 4)
                yield return new IdentifierToken("buttonlist");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("move_child");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("waypoint_btn");
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(4));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                // waypoint_btn.connect("pressed", self, "_on_waypoints_pressed")
                yield return new IdentifierToken("waypoint_btn");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("connect");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("pressed"));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.Self);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new StringVariant("_on_waypoints_pressed"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                
                // add_child(Waypoints.MENU_SCENE.instance())
                yield return new IdentifierToken("add_child");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("Waypoints");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("MENU_SCENE");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("instance");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);
                
                // buttonlist.margin_top -= 24
                yield return new IdentifierToken("buttonlist");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("margin_top");
                yield return new Token(TokenType.OpAssignSub);
                yield return new ConstantToken(new IntVariant(24));
                yield return new Token(TokenType.Newline, 1);
                
                // buttonlist.margin_bottom += 24
                yield return new IdentifierToken("buttonlist");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("margin_bottom");
                yield return new Token(TokenType.OpAssignAdd);
                yield return new ConstantToken(new IntVariant(24));
                yield return new Token(TokenType.Newline, 1);
            }
            else yield return token;
        }
    }
}
