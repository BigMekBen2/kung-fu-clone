using Raylib_cs;
using KungFuClone.Entities.Enemies;

namespace KungFuClone.Rendering;

public static class EnemyRenderer
{
    private static readonly Color Skin      = new(0xE0, 0xA0, 0x60, 255);
    private static readonly Color DarkGray  = new(0x40, 0x40, 0x40, 255);
    private static readonly Color Red       = new(0xCC, 0x22, 0x22, 255);
    private static readonly Color Brown     = new(0x60, 0x30, 0x10, 255);
    private static readonly Color Yellow    = new(0xFF, 0xCC, 0x00, 255);
    private static readonly Color Purple    = new(0x80, 0x00, 0x80, 255);
    private static readonly Color Gold      = new(0xFF, 0xD7, 0x00, 255);
    private static readonly Color HurtTint  = new(0xFF, 0x60, 0x60, 180);

    public static void Draw(Enemy e, float cameraX)
    {
        if (!e.Active) return;

        // cameraX is already set in Renderer.CameraX — GameScreen passes the
        // renderer's current cameraX via the static Raylib drawing call offset.
        // We read it directly from the last set value.
        float cam = Renderer.CurrentCameraX;

        int sx = (int)(e.Position.X - cam);
        int sy = (int)e.Position.Y;
        int dir = e.FacingRight ? 1 : -1;

        switch (e)
        {
            case Gripper g:     DrawGripper(sx, sy, dir, g.EnemyStateValue); break;
            case KnifeThrower:  DrawKnifeThrower(sx, sy, dir); break;
            case StickFighter:  DrawStickFighter(sx, sy, dir); break;
            case Bouncer b:     DrawBouncer(sx, sy, dir, b.Health); break;
            case Pudgy:         DrawPudgy(sx, sy, dir); break;
            case MrX mx:        DrawMrX(sx, sy, dir, mx.Health); break;
        }
    }

    private static void DrawGripper(int sx, int sy, int dir, EnemyState state)
    {
        // Red gi
        Raylib.DrawRectangle(sx + 3, sy + 8,  10, 8, Red);
        Raylib.DrawRectangle(sx + 4, sy + 16, 4, 8, DarkGray);
        Raylib.DrawRectangle(sx + 8, sy + 16, 4, 8, DarkGray);
        Raylib.DrawCircle(sx + 8, sy + 4, 4, Skin);
        Raylib.DrawRectangle(sx + 4, sy, 8, 3, DarkGray); // hair
        // Grabbing arms
        if (state == EnemyState.Attacking)
        {
            int ax = dir > 0 ? sx + 14 : sx - 8;
            Raylib.DrawRectangle(ax, sy + 8, 8, 4, Skin);
            Raylib.DrawRectangle(ax, sy + 12, 8, 4, Skin);
        }
    }

    private static void DrawKnifeThrower(int sx, int sy, int dir)
    {
        // Blue outfit
        Raylib.DrawRectangle(sx + 3, sy + 8, 10, 8, new Color(0x20, 0x20, 0xA0, 255));
        Raylib.DrawRectangle(sx + 4, sy + 16, 4, 8, DarkGray);
        Raylib.DrawRectangle(sx + 8, sy + 16, 4, 8, DarkGray);
        Raylib.DrawCircle(sx + 8, sy + 4, 4, Skin);
        Raylib.DrawRectangle(sx + 4, sy, 8, 3, Brown);
        // Throwing arm
        int ax = dir > 0 ? sx + 13 : sx - 5;
        Raylib.DrawRectangle(ax, sy + 8, 5, 3, Skin);
    }

    private static void DrawStickFighter(int sx, int sy, int dir)
    {
        // Green outfit
        Raylib.DrawRectangle(sx + 3, sy + 8, 10, 8, new Color(0x10, 0x80, 0x30, 255));
        Raylib.DrawRectangle(sx + 4, sy + 16, 4, 8, Brown);
        Raylib.DrawRectangle(sx + 8, sy + 16, 4, 8, Brown);
        Raylib.DrawCircle(sx + 8, sy + 4, 4, Skin);
        Raylib.DrawRectangle(sx + 4, sy, 8, 3, DarkGray);
        // Staff
        int stx = dir > 0 ? sx + 14 : sx - 6;
        Raylib.DrawRectangle(stx, sy - 2, 2, 26, Brown);
    }

    private static void DrawBouncer(int sx, int sy, int dir, int health)
    {
        // Large white outfit, bigger body
        Raylib.DrawRectangle(sx + 2, sy + 8, 14, 10, new Color(0xF0, 0xF0, 0xF0, 255));
        Raylib.DrawRectangle(sx + 3, sy + 18, 5, 10, DarkGray);
        Raylib.DrawRectangle(sx + 8, sy + 18, 5, 10, DarkGray);
        Raylib.DrawCircle(sx + 9, sy + 4, 5, Skin);
        Raylib.DrawRectangle(sx + 4, sy, 10, 3, DarkGray);
        // Health pips
        for (int i = 0; i < health; i++)
            Raylib.DrawRectangle(sx + 3 + i * 5, sy - 4, 4, 3, new Color(0xFF, 0x80, 0x00, 255));
    }

    private static void DrawPudgy(int sx, int sy, int dir)
    {
        // Orange outfit, slightly bigger than gripper
        Raylib.DrawRectangle(sx + 2, sy + 8, 12, 8, new Color(0xFF, 0x70, 0x00, 255));
        Raylib.DrawRectangle(sx + 3, sy + 16, 4, 8, DarkGray);
        Raylib.DrawRectangle(sx + 8, sy + 16, 4, 8, DarkGray);
        Raylib.DrawCircle(sx + 8, sy + 4, 5, Skin); // bigger head
        Raylib.DrawRectangle(sx + 3, sy, 10, 3, DarkGray);
    }

    private static void DrawMrX(int sx, int sy, int dir, int health)
    {
        // Purple + gold — larger, imposing
        Raylib.DrawRectangle(sx + 2, sy + 8, 16, 10, Purple);
        Raylib.DrawRectangle(sx + 4, sy + 2, 12, 8, Purple); // cape extension
        Raylib.DrawRectangle(sx + 3, sy + 18, 5, 10, DarkGray);
        Raylib.DrawRectangle(sx + 10, sy + 18, 5, 10, DarkGray);
        Raylib.DrawCircle(sx + 10, sy + 4, 5, Skin);
        Raylib.DrawRectangle(sx + 5, sy, 10, 3, DarkGray); // hair
        // Gold belt
        Raylib.DrawRectangle(sx + 2, sy + 16, 16, 2, Gold);
        // Health bar above
        for (int i = 0; i < health; i++)
            Raylib.DrawRectangle(sx + 1 + i * 4, sy - 6, 3, 3, Red);
    }
}
