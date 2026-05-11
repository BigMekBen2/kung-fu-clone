using Raylib_cs;
using KungFuClone.Core;
using KungFuClone.Rendering;

namespace KungFuClone.Screens;

public class TitleScreen : IScreen
{
    private readonly Game     _game;
    private readonly Renderer _r;
    private readonly InputManager _input;
    private float _blink;

    public TitleScreen(Game game, Renderer r, InputManager input)
    {
        _game  = game;
        _r     = r;
        _input = input;
    }

    public void OnEnter()  { _blink = 0f; }
    public void OnExit()   { }

    public void Update(float dt)
    {
        _blink += dt;
        if (_input.Start)
            _game.TransitionTo(GameStateId.Playing);
    }

    public void Draw()
    {
        _r.BeginFrame();
        DrawTitle();
        _r.EndFrame();
    }

    private void DrawTitle()
    {
        // Sky background
        _r.FillRectScreen(0, 0, Constants.InternalWidth, Constants.InternalHeight,
            new Color(0x10, 0x10, 0x50, 255));

        // Ground strip
        _r.FillRectScreen(0, Constants.GroundY, Constants.InternalWidth,
            Constants.InternalHeight - Constants.GroundY, new Color(0x40, 0x28, 0x10, 255));

        // "KUNG FU" — blocky pixel letters drawn as rectangles
        DrawBigText("KUNG FU", 28, 60, new Color(0xFF, 0xD0, 0x00, 255));
        DrawBigText("MASTER",  44, 88, new Color(0xFF, 0x60, 0x00, 255));

        // Blinking prompt
        if ((int)(_blink * 2) % 2 == 0)
            _r.DrawText("PRESS ENTER", 84, 160, 8, Color.White);

        // Hi-score line
        _r.DrawText("HI-SCORE  00000", 56, 14, 8, new Color(0xFF, 0xFF, 0x00, 255));

        // Copyright-ish
        _r.DrawText("(C) 1984 IREM", 72, 224, 8, new Color(0x88, 0x88, 0x88, 255));
    }

    // Draw text using large 8x8 blocks for NES-style feel
    private void DrawBigText(string text, int sx, int sy, Color c)
    {
        // Use raylib built-in text at size 16 — close enough for title
        Raylib.DrawText(text, sx, sy, 16, c);
        // Outline
        Raylib.DrawText(text, sx + 1, sy + 1, 16, new Color(0x40, 0x20, 0x00, 200));
    }
}
