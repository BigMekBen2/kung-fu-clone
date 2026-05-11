using Raylib_cs;
using System.Numerics;
using KungFuClone.Core;

namespace KungFuClone.Rendering;

public class Renderer
{
    private RenderTexture2D _canvas;
    public  float CameraX;

    public void Init()
    {
        _canvas = Raylib.LoadRenderTexture(Constants.InternalWidth, Constants.InternalHeight);
    }

    public void Unload()
    {
        Raylib.UnloadRenderTexture(_canvas);
    }

    public void BeginFrame()
    {
        Raylib.BeginTextureMode(_canvas);
        Raylib.ClearBackground(Color.Black);
    }

    public void EndFrame()
    {
        Raylib.EndTextureMode();
        // Flip Y — raylib render textures are stored upside-down
        var src  = new Rectangle(0, 0,  Constants.InternalWidth, -Constants.InternalHeight);
        var dest = new Rectangle(0, 0,  Constants.WindowWidth,    Constants.WindowHeight);
        Raylib.DrawTexturePro(_canvas.Texture, src, dest, Vector2.Zero, 0f, Color.White);
    }

    // World-space helpers (subtract CameraX for screen X)
    public void FillRect(float wx, float wy, int w, int h, Color c)
        => Raylib.DrawRectangle((int)(wx - CameraX), (int)wy, w, h, c);

    public void FillRectScreen(int sx, int sy, int w, int h, Color c)
        => Raylib.DrawRectangle(sx, sy, w, h, c);

    public void DrawText(string text, int sx, int sy, int fontSize, Color c)
        => Raylib.DrawText(text, sx, sy, fontSize, c);
}
