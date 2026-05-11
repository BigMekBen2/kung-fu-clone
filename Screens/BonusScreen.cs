using Raylib_cs;
using KungFuClone.Core;
using KungFuClone.Rendering;

namespace KungFuClone.Screens;

// Placeholder — fleshed out in Phase 7
public class BonusScreen : IScreen
{
    private readonly Game         _game;
    private readonly Renderer     _r;
    private readonly InputManager _input;
    private readonly GameContext  _ctx;
    private float _timer;

    public BonusScreen(Game game, Renderer r, InputManager input, GameContext ctx)
    {
        _game = game; _r = r; _input = input; _ctx = ctx;
    }

    public void OnEnter()  { _timer = 0f; }
    public void OnExit()   { }

    public void Update(float dt)
    {
        _timer += dt;
        if (_timer > 3f || _input.Start)
            _game.TransitionTo(GameStateId.Playing);
    }

    public void Draw()
    {
        _r.BeginFrame();
        _r.FillRectScreen(0, 0, Constants.InternalWidth, Constants.InternalHeight, Color.Black);
        _r.DrawText("BONUS STAGE", 76, 100, 10, new Color(0xFF,0xD0,0x00,255));
        _r.DrawText("(coming soon)", 72, 118, 8, Color.Gray);
        _r.EndFrame();
    }
}
