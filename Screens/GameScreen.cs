using Raylib_cs;
using KungFuClone.Core;
using KungFuClone.Rendering;

namespace KungFuClone.Screens;

// Placeholder — fleshed out in Phase 2+
public class GameScreen : IScreen
{
    private readonly Game         _game;
    private readonly Renderer     _r;
    private readonly InputManager _input;
    private readonly GameContext  _ctx;

    public GameScreen(Game game, Renderer r, InputManager input, GameContext ctx)
    {
        _game  = game;
        _r     = r;
        _input = input;
        _ctx   = ctx;
    }

    public void OnEnter()  { }
    public void OnExit()   { }

    public void Update(float dt) { }

    public void Draw()
    {
        _r.BeginFrame();
        _r.FillRectScreen(0, 0, Constants.InternalWidth, Constants.InternalHeight,
            new Color(0x10, 0x10, 0x50, 255));
        _r.DrawText("FLOOR " + _ctx.CurrentFloor, 90, 110, 10, Color.White);
        _r.EndFrame();
    }
}
