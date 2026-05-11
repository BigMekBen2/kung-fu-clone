using System.Numerics;
using Raylib_cs;
using KungFuClone.Core;
using KungFuClone.Entities;
using KungFuClone.Rendering;

namespace KungFuClone.Screens;

public class GameScreen : IScreen
{
    private readonly Game         _game;
    private readonly Renderer     _r;
    private readonly InputManager _input;
    private readonly GameContext  _ctx;

    public  Player   Player { get; private set; } = null!;
    private float    _cameraX;
    private float    _floorWidth = 3200f;

    public GameScreen(Game game, Renderer r, InputManager input, GameContext ctx)
    {
        _game  = game;
        _r     = r;
        _input = input;
        _ctx   = ctx;
    }

    public void OnEnter()
    {
        Player   = new Player();
        Player.Position = new Vector2(Constants.PlayerStartX, Constants.PlayerStartY);
        Player.Lives    = _ctx.Lives;
        Player.Health   = 3;
        _cameraX        = 0f;
        _ctx.FloorTimeRemaining = 120f;
    }

    public void OnExit() { }

    public void Update(float dt)
    {
        if (_input.Start) _paused = !_paused;
        if (_paused) return;

        _ctx.FloorTimeRemaining -= dt;

        Player.Update(dt, _input);

        // Scroll camera
        _cameraX += Constants.ScrollSpeed * dt;
        _cameraX  = Math.Min(_cameraX, _floorWidth - Constants.InternalWidth);

        // Player can't fall behind left edge
        float leftBound = _cameraX + 4f;
        if (Player.Position.X < leftBound)
        {
            Player.Position.X = leftBound;
            if (Player.Velocity.X < 0) Player.Velocity.X = 0;
        }

        // Player can't go past right edge
        float rightBound = _cameraX + Constants.InternalWidth - 20f;
        if (Player.Position.X > rightBound)
            Player.Position.X = rightBound;

        _r.CameraX = _cameraX;

        // Player died — lose life
        if (Player.State == PlayerState.Dead && Player.StateTimer <= 0f)
        {
            _ctx.Lives--;
            _ctx.Score = Player.Health; // preserve score etc.
            if (_ctx.Lives <= 0)
                _game.TransitionTo(GameStateId.GameOver);
            else
                OnEnter(); // restart floor
        }

        // Floor complete when camera reaches end
        if (_cameraX >= _floorWidth - Constants.InternalWidth - 1f)
        {
            _ctx.CurrentFloor++;
            if (_ctx.CurrentFloor > 5)
            {
                // Won — loop back
                _ctx.CurrentFloor = 1;
                _game.TransitionTo(GameStateId.Title);
            }
            else
            {
                _game.TransitionTo(GameStateId.BonusStage);
            }
        }

    }

    private bool _paused;

    public void Draw()
    {
        _r.BeginFrame();

        BackgroundRenderer.Draw(_ctx.CurrentFloor, _cameraX);
        PlayerRenderer.Draw(Player, _cameraX);
        HudRenderer.Draw(_ctx, Player);

        if (_paused)
        {
            Raylib.DrawRectangle(88, 108, 80, 14, new Color(0,0,0,180));
            Raylib.DrawText("PAUSED", 100, 110, 10, Color.White);
        }

        _r.EndFrame();
    }
}
