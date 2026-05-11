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

        _floorDef   = LevelLoader.Build(_ctx.CurrentFloor);
        _floorWidth = _floorDef.TotalWidth;
        _paused     = false;
        _floorDone  = false;

        Player = new Player();
        // Floor 1 scrolls left: start at right end; others start at left
        float startX = _floorDef.Direction < 0
            ? _floorWidth - Constants.PlayerStartX - Constants.InternalWidth / 2f
            : Constants.PlayerStartX;
        Player.Position  = new Vector2(startX, Constants.PlayerStartY);
        Player.Lives     = _ctx.Lives;
        Player.Health    = 3;
        Player.FacingRight = _floorDef.Direction >= 0;

        Enemies.Clear();
        Projectiles.Clear();

        // Camera centered on player, clamped to floor
        _cameraX = Math.Clamp(Player.Position.X - Constants.InternalWidth / 2f,
                              0, _floorWidth - Constants.InternalWidth);

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

        foreach (var e in Enemies.ToList()) e.Update(dt, this);
        foreach (var p in Projectiles.ToList()) p.Update(dt);

        int healthBefore = Player.Health;
        _collision.Resolve(Player, Enemies, Projectiles, _score);
        if (Player.Health < healthBefore) _hitFlashTimer = 0.1f;
        _hitFlashTimer -= dt;

        // Camera follows player, centered on X
        _cameraX = Math.Clamp(
            Player.Position.X - Constants.InternalWidth / 2f,
            0, _floorWidth - Constants.InternalWidth);
        _r.CameraX = _cameraX;

        // Hard world boundaries — player can't leave the floor
        if (Player.Position.X < 8f) { Player.Position.X = 8f; Player.Velocity.X = 0; }
        if (Player.Position.X > _floorWidth - 8f) { Player.Position.X = _floorWidth - 8f; Player.Velocity.X = 0; }

        _spawner.Update(_cameraX, Enemies);

        // Sync lives to context
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

        // Floor complete: player reaches the far end (direction-aware) with no enemies
        bool atEnd = _floorDef.Direction >= 0
            ? Player.Position.X >= _floorWidth - 40f
            : Player.Position.X <= 40f;

        if (!_floorDone && atEnd && Enemies.Count == 0)
        {
            _floorDone = true;
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
