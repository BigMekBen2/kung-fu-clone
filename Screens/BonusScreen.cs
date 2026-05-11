using System.Numerics;
using Raylib_cs;
using KungFuClone.Audio;
using KungFuClone.Core;
using KungFuClone.Rendering;

namespace KungFuClone.Screens;

public class BonusScreen : IScreen
{
    private readonly Game         _game;
    private readonly Renderer     _r;
    private readonly InputManager _input;
    private readonly GameContext  _ctx;

    private const float Duration   = 30f;
    private const float BagScore   = 500;
    private const int   MaxMisses  = 10;

    private float  _playerX;
    private int    _caught;
    private int    _missed;
    private float  _elapsed;
    private List<FallingBag> _bags = new();
    private float  _spawnTimer;
    private float  _spawnInterval = 1.4f;
    private Random _rng = new();
    private int    _earnedScore;

    private struct FallingBag
    {
        public float X, Y;
        public bool  Caught;
        public float FallSpeed;
    }

    public BonusScreen(Game game, Renderer r, InputManager input, GameContext ctx)
    {
        _game = game; _r = r; _input = input; _ctx = ctx;
    }

    public void OnEnter()
    {
        _playerX      = Constants.InternalWidth / 2f - 8;
        _caught       = 0;
        _missed       = 0;
        _elapsed      = 0f;
        _earnedScore  = 0;
        _bags.Clear();
        _spawnTimer   = 0.5f;
        _spawnInterval = 1.4f;
    }

    public void OnExit()
    {
        _ctx.Score += _earnedScore;
        if (_ctx.Score > _ctx.HiScore) _ctx.HiScore = _ctx.Score;
    }

    public void Update(float dt)
    {
        _elapsed += dt;

        if (_input.Left  && _playerX > 8f)          _playerX -= 80f * dt;
        if (_input.Right && _playerX < Constants.InternalWidth - 24f) _playerX += 80f * dt;

        // Spawn bags
        _spawnTimer -= dt;
        if (_spawnTimer <= 0f)
        {
            _spawnTimer    = _spawnInterval;
            _spawnInterval = Math.Max(0.5f, _spawnInterval - 0.02f); // speed up over time
            float bx = 10f + (float)_rng.NextDouble() * (Constants.InternalWidth - 20f);
            _bags.Add(new FallingBag { X = bx, Y = 10f, FallSpeed = 80f + (float)_rng.NextDouble() * 40f });
        }

        // Update bags
        float catchY = Constants.GroundY - 28f; // player height
        for (int i = 0; i < _bags.Count; i++)
        {
            var b = _bags[i];
            if (b.Caught) { _bags.RemoveAt(i--); continue; }

            b.Y += b.FallSpeed * dt;

            // Catch check
            if (b.Y >= catchY && b.Y <= catchY + 20f)
            {
                if (MathF.Abs(b.X - _playerX) < 20f)
                {
                    b.Caught = true;
                    _caught++;
                    _earnedScore += (int)BagScore;
                    AudioEngine.BonusBag.Play();
                    _bags.RemoveAt(i--);
                    continue;
                }
            }

            // Miss
            if (b.Y > Constants.InternalHeight)
            {
                _missed++;
                _bags.RemoveAt(i--);
                continue;
            }

            _bags[i] = b;
        }

        if (_elapsed >= Duration || _missed >= MaxMisses)
            _game.TransitionTo(GameStateId.Playing);
    }

    public void Draw()
    {
        _r.BeginFrame();

        // Background
        _r.FillRectScreen(0, 0, Constants.InternalWidth, Constants.InternalHeight,
            new Color(0x10, 0x18, 0x30, 255));
        // Ground
        _r.FillRectScreen(0, Constants.GroundY, Constants.InternalWidth,
            Constants.InternalHeight - Constants.GroundY, new Color(0x40, 0x28, 0x10, 255));

        // Title
        Raylib.DrawText("BONUS STAGE", 76, 8, 10, new Color(0xFF, 0xD0, 0x00, 255));
        Raylib.DrawText($"CAUGHT: {_caught}  MISSED: {_missed}/{MaxMisses}", 40, 22, 8, Color.White);

        float timeLeft = Duration - _elapsed;
        Raylib.DrawText($"TIME {(int)timeLeft}", 200, 8, 8, Color.Yellow);

        // Bags
        foreach (var b in _bags)
            DrawBag((int)b.X, (int)b.Y);

        // Player (Thomas) — simple silhouette
        DrawPlayer((int)_playerX, Constants.GroundY - 24);

        // Score earned so far
        Raylib.DrawText($"+{_earnedScore}", 100, Constants.InternalHeight - 16, 8, Color.LightGray);

        _r.EndFrame();
    }

    private static void DrawBag(int x, int y)
    {
        Raylib.DrawRectangle(x, y, 12, 10, new Color(0xA0, 0x70, 0x30, 255));
        Raylib.DrawRectangle(x + 1, y - 3, 10, 4, new Color(0x80, 0x50, 0x10, 255));
    }

    private static void DrawPlayer(int x, int y)
    {
        Raylib.DrawRectangle(x + 3, y + 8, 10, 8, Color.White);
        Raylib.DrawRectangle(x + 3, y + 14, 10, 2, new Color(0xFF, 0x88, 0x00, 255));
        Raylib.DrawRectangle(x + 4, y + 16, 4, 8, Color.White);
        Raylib.DrawRectangle(x + 8, y + 16, 4, 8, Color.White);
        Raylib.DrawCircle(x + 8, y + 4, 4, new Color(0xF0, 0xB0, 0x70, 255));
    }
}
