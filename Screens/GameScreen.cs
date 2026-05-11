using System.Numerics;
using Raylib_cs;
using KungFuClone.Core;
using KungFuClone.Entities;
using KungFuClone.Entities.Enemies;
using KungFuClone.Level;
using KungFuClone.Rendering;
using KungFuClone.Systems;

namespace KungFuClone.Screens;

public class GameScreen : IScreen
{
    private readonly Game         _game;
    private readonly Renderer     _r;
    private readonly InputManager _input;
    private readonly GameContext  _ctx;

    public  Player          Player      { get; private set; } = null!;
    public  List<Enemy>     Enemies     { get; } = new();
    public  List<Projectile> Projectiles { get; } = new();

    private float          _cameraX;
    private float          _floorWidth;
    private bool           _paused;
    private bool           _floorDone;
    private float          _hitFlashTimer;

    private FloorDefinition  _floorDef  = null!;
    private SpawnSystem      _spawner   = new();
    private CollisionSystem  _collision = new();
    private ScoreSystem      _score     = null!;

    public GameScreen(Game game, Renderer r, InputManager input, GameContext ctx)
    {
        _game  = game;
        _r     = r;
        _input = input;
        _ctx   = ctx;
    }

    public void OnEnter()
    {
        _game.Music.Init(_ctx.CurrentFloor);

        Player = new Player();
        Player.Position = new Vector2(Constants.PlayerStartX, Constants.PlayerStartY);
        Player.Lives    = _ctx.Lives;
        Player.Health   = 3;

        Enemies.Clear();
        Projectiles.Clear();

        _floorDef  = LevelLoader.Build(_ctx.CurrentFloor);
        _floorWidth = _floorDef.TotalWidth;
        _cameraX   = 0f;
        _paused    = false;
        _floorDone = false;

        _spawner.LoadFloor(_floorDef);
        _score = new ScoreSystem(_ctx);
        _ctx.FloorTimeRemaining = _floorDef.TimeLimit;
    }

    public void OnExit() { }

    public void Update(float dt)
    {
        if (_input.Start) { _paused = !_paused; return; }
        if (_paused) return;

        _ctx.FloorTimeRemaining -= dt;

        Player.Update(dt, _input);

        foreach (var e in Enemies) e.Update(dt, this);
        foreach (var p in Projectiles) p.Update(dt);

        int healthBefore = Player.Health;
        _collision.Resolve(Player, Enemies, Projectiles, _score);
        if (Player.Health < healthBefore) _hitFlashTimer = 0.1f;
        _hitFlashTimer -= dt;

        // Scroll camera
        _cameraX += Constants.ScrollSpeed * dt;
        _cameraX  = Math.Min(_cameraX, _floorWidth - Constants.InternalWidth);

        // Player left boundary
        float leftBound = _cameraX + 4f;
        if (Player.Position.X < leftBound) { Player.Position.X = leftBound; if (Player.Velocity.X < 0) Player.Velocity.X = 0; }

        // Player right boundary
        float rightBound = _cameraX + Constants.InternalWidth - 20f;
        if (Player.Position.X > rightBound) Player.Position.X = rightBound;

        _r.CameraX = _cameraX;

        _spawner.Update(_cameraX, Enemies, Enemies);

        // Sync lives/score to context
        _ctx.Lives = Player.Lives;

        // Player dead
        if (Player.State == PlayerState.Dead && Player.StateTimer <= 0f)
        {
            _ctx.Lives--;
            if (_ctx.Lives <= 0)
            {
                Audio.AudioEngine.GameOverSfx.Play();
                _game.TransitionTo(GameStateId.GameOver);
            }
            else
                OnEnter();
            return;
        }

        // Floor complete: camera at far right AND all enemies gone
        if (!_floorDone &&
            _cameraX >= _floorWidth - Constants.InternalWidth - 1f &&
            Enemies.Count == 0)
        {
            _floorDone = true;
            // Time bonus
            _score.Add((int)(_ctx.FloorTimeRemaining * 10));
            _ctx.CurrentFloor++;
            if (_ctx.CurrentFloor > 5)
            {
                _ctx.CurrentFloor = 1;
                _game.TransitionTo(GameStateId.Title);
            }
            else
            {
                _game.TransitionTo(GameStateId.BonusStage);
            }
        }
    }

    public void Draw()
    {
        _r.BeginFrame();

        BackgroundRenderer.Draw(_ctx.CurrentFloor, _cameraX);

        foreach (var p in Projectiles) p.Draw(_cameraX);
        foreach (var e in Enemies)     DrawEnemy(e);
        PlayerRenderer.Draw(Player, _cameraX);
        HudRenderer.Draw(_ctx, Player);

        if (_hitFlashTimer > 0f)
            Raylib.DrawRectangle(0, 0, Constants.InternalWidth, Constants.InternalHeight,
                new Color(255, 255, 255, 80));

        if (_paused)
        {
            Raylib.DrawRectangle(88, 108, 80, 14, new Color(0, 0, 0, 180));
            Raylib.DrawText("PAUSED", 100, 110, 10, Color.White);
        }

        _r.EndFrame();
    }

    private static void DrawEnemy(Enemy e)
    {
        EnemyRenderer.Draw(e, 0); // cameraX handled via Renderer.CameraX — pass 0 since we draw via world coords
    }
}
