using Raylib_cs;
using KungFuClone.Core;
using KungFuClone.Entities;

namespace KungFuClone.Rendering;

public static class HudRenderer
{
    private static readonly Color HeartFull  = new(0xFF, 0x20, 0x20, 255);
    private static readonly Color HeartEmpty = new(0x40, 0x20, 0x20, 255);

    public static void Draw(GameContext ctx, Player player)
    {
        // Hearts (top-left)
        for (int i = 0; i < 3; i++)
        {
            Color c = i < player.Health ? HeartFull : HeartEmpty;
            DrawHeart(4 + i * 10, 4, c);
        }

        // Score (top-center)
        DrawScore(ctx.Score, 88, 4);

        // Hi-score label + value (top-right)
        Raylib.DrawText("HI", 188, 4, 6, new Color(0xFF,0xFF,0x00,255));
        DrawScore(ctx.HiScore, 196, 4);

        // Lives
        Raylib.DrawText($"x{player.Lives}", 4, 228, 8, Color.White);

        // Floor timer
        Raylib.DrawText($"T {(int)ctx.FloorTimeRemaining:D3}", 200, 228, 8, Color.White);

        // Floor
        Raylib.DrawText($"FL{ctx.CurrentFloor}", 110, 228, 8, new Color(0xFF,0xD0,0x00,255));
    }

    private static void DrawHeart(int x, int y, Color c)
    {
        Raylib.DrawRectangle(x,   y+1, 3, 3, c);
        Raylib.DrawRectangle(x+4, y+1, 3, 3, c);
        Raylib.DrawRectangle(x+1, y,   5, 3, c);
        Raylib.DrawRectangle(x+1, y+2, 5, 4, c);
        Raylib.DrawRectangle(x+2, y+5, 3, 1, c);
    }

    private static void DrawScore(int score, int x, int y)
    {
        // Simple right-aligned 6-digit score using DrawText
        Raylib.DrawText($"{score:D6}", x, y, 8, Color.White);
    }
}
