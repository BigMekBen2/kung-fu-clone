using Raylib_cs;
using KungFuClone.Core;
using KungFuClone.Rendering;

namespace KungFuClone.Screens;

public class GameOverScreen : IScreen
{
    private readonly Game         _game;
    private readonly Renderer     _r;
    private readonly InputManager _input;
    private readonly GameContext  _ctx;
    private float _timer;

    public GameOverScreen(Game game, Renderer r, InputManager input, GameContext ctx)
    {
        _game  = game;
        _r     = r;
        _input = input;
        _ctx   = ctx;
    }

    public void OnEnter()  { _timer = 0f; }
    public void OnExit()   { }

    public void Update(float dt)
    {
        _timer += dt;
        if (_input.Start || _timer > 5f)
            _game.TransitionTo(GameStateId.Title);
    }

    public void Draw()
    {
        _r.BeginFrame();
        _r.FillRectScreen(0, 0, Constants.InternalWidth, Constants.InternalHeight, Color.Black);
        _r.DrawText("GAME OVER", 82, 100, 14, Color.Red);
        _r.DrawText($"SCORE  {_ctx.Score:D6}", 72, 124, 8, Color.White);
        _r.DrawText("PRESS ENTER", 80, 150, 8, Color.Gray);
        _r.EndFrame();
    }
}
