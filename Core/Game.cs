using Raylib_cs;
using KungFuClone.Audio;
using KungFuClone.Rendering;
using KungFuClone.Screens;

namespace KungFuClone.Core;

public class Game
{
    private readonly Renderer     _renderer = new();
    private readonly InputManager _input    = new();
    private readonly GameContext  _ctx      = new();
    private readonly AudioEngine  _audio    = new();

    private IScreen _current = null!;

    private TitleScreen    _titleScreen    = null!;
    private GameScreen     _gameScreen     = null!;
    private BonusScreen    _bonusScreen    = null!;
    private GameOverScreen _gameOverScreen = null!;

    public void Run()
    {
        Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
        Raylib.InitWindow(Constants.WindowWidth, Constants.WindowHeight, "Kung-Fu Master");
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);

        _renderer.Init();
        _audio.BakeAll();

        _titleScreen    = new TitleScreen(this, _renderer, _input);
        _gameScreen     = new GameScreen(this, _renderer, _input, _ctx);
        _bonusScreen    = new BonusScreen(this, _renderer, _input, _ctx);
        _gameOverScreen = new GameOverScreen(this, _renderer, _input, _ctx);

        TransitionTo(GameStateId.Title);

        while (!Raylib.WindowShouldClose())
        {
            float dt = Raylib.GetFrameTime();
            _current.Update(dt);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            _current.Draw();
            Raylib.EndDrawing();
        }

        _renderer.Unload();
        _audio.UnloadAll();
        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }

    public void TransitionTo(GameStateId next)
    {
        _current?.OnExit();
        _current = next switch
        {
            GameStateId.Title      => _titleScreen,
            GameStateId.Playing    => _gameScreen,
            GameStateId.BonusStage => _bonusScreen,
            GameStateId.GameOver   => _gameOverScreen,
            _                      => _titleScreen
        };
        _current.OnEnter();
    }
}
