using Raylib_cs;
using KungFuClone.Core;

namespace KungFuClone.Rendering;

public static class BackgroundRenderer
{
    // Per-floor palettes: sky, wall, floor, accent
    private static readonly (Color sky, Color wall, Color ground, Color accent)[] Palettes =
    {
        (new Color(0x10,0x10,0x50,255), new Color(0x28,0x20,0x60,255), new Color(0x40,0x28,0x10,255), new Color(0xFF,0xD0,0x00,255)), // F1
        (new Color(0x00,0x10,0x40,255), new Color(0x20,0x10,0x50,255), new Color(0x30,0x20,0x10,255), new Color(0xFF,0x80,0x00,255)), // F2
        (new Color(0x10,0x20,0x10,255), new Color(0x10,0x30,0x10,255), new Color(0x20,0x28,0x08,255), new Color(0x00,0xFF,0x80,255)), // F3
        (new Color(0x30,0x10,0x00,255), new Color(0x50,0x18,0x00,255), new Color(0x30,0x18,0x00,255), new Color(0xFF,0x40,0x00,255)), // F4
        (new Color(0x10,0x00,0x10,255), new Color(0x30,0x00,0x30,255), new Color(0x20,0x10,0x20,255), new Color(0xFF,0x00,0x80,255)), // F5
    };

    public static void Draw(int floor, float cameraX)
    {
        int pi = Math.Clamp(floor - 1, 0, Palettes.Length - 1);
        var (sky, wall, ground, accent) = Palettes[pi];

        // Layer 0: sky fill
        Raylib.DrawRectangle(0, 0, Constants.InternalWidth, Constants.GroundY, sky);

        // Layer 1: slow-scroll architectural pillars (0.4x parallax)
        float px1 = cameraX * 0.4f;
        DrawPillars(px1, wall, accent);

        // Layer 2: full-scroll floor detail (1:1)
        DrawFloorDetail(cameraX, wall, accent);

        // Ground strip
        Raylib.DrawRectangle(0, Constants.GroundY, Constants.InternalWidth,
            Constants.InternalHeight - Constants.GroundY, ground);
        // Ground highlight line
        Raylib.DrawRectangle(0, Constants.GroundY, Constants.InternalWidth, 1, accent);
    }

    private static void DrawPillars(float scrollX, Color wall, Color accent)
    {
        int spacing = 48;
        int startPillar = (int)(scrollX / spacing) * spacing;
        for (int x = startPillar - spacing; x < Constants.InternalWidth + spacing * 2; x += spacing)
        {
            int sx = x - (int)scrollX % spacing;
            // Pillar
            Raylib.DrawRectangle(sx, 20, 8, Constants.GroundY - 20, wall);
            // Pillar cap
            Raylib.DrawRectangle(sx - 2, 20, 12, 4, accent);
        }
    }

    private static void DrawFloorDetail(float cameraX, Color wall, Color accent)
    {
        // Repeating arch motif every 80px
        int tileW = 80;
        int startTile = (int)(cameraX / tileW) * tileW;
        for (int wx = startTile; wx < cameraX + Constants.InternalWidth + tileW; wx += tileW)
        {
            int sx = (int)(wx - cameraX);
            // Horizontal band
            Raylib.DrawRectangle(sx, 40, tileW, 3, wall);
            // Small decorative block
            Raylib.DrawRectangle(sx + 30, 24, 20, 14, wall);
            Raylib.DrawRectangle(sx + 32, 26, 16, 10, accent);
        }
    }
}
