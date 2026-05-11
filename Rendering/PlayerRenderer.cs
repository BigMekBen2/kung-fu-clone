using Raylib_cs;
using System.Numerics;
using KungFuClone.Entities;

namespace KungFuClone.Rendering;

public static class PlayerRenderer
{
    private static readonly Color Skin   = new(0xF0, 0xB0, 0x70, 255);
    private static readonly Color Shirt  = new(0xFF, 0xFF, 0xFF, 255);  // white gi top
    private static readonly Color Pants  = new(0xFF, 0xFF, 0xFF, 255);
    private static readonly Color Belt   = new(0xFF, 0x88, 0x00, 255);
    private static readonly Color Hair   = new(0x20, 0x10, 0x00, 255);
    private static readonly Color Shoe   = new(0x30, 0x18, 0x08, 255);
    private static readonly Color HurtTint = new(0xFF, 0x60, 0x60, 200);

    // Walk cycle: leg offsets (frontLeg, backLeg) relative to hip
    private static readonly (int, int)[] WalkFrames = { (0,0),(2,-2),(4,-4),(2,-2) };

    public static void Draw(Player p, float cameraX)
    {
        if (!p.Active) return;

        int sx = (int)(p.Position.X - cameraX);
        int sy = (int)p.Position.Y;
        int dir = p.FacingRight ? 1 : -1;

        // Flicker during invincibility
        if (p.InvincibleTimer > 0f && ((int)(p.InvincibleTimer * 12) % 2 == 0)) return;

        switch (p.State)
        {
            case PlayerState.Idle:
                DrawStand(sx, sy, dir, 0);
                break;
            case PlayerState.Walking:
                int wf = (int)(Raylib.GetTime() * 8) % 4;
                DrawStand(sx, sy, dir, wf);
                break;
            case PlayerState.Crouching:
            case PlayerState.CrouchPunch:
            case PlayerState.CrouchKick:
                DrawCrouch(sx, sy, dir, p.State);
                break;
            case PlayerState.Jumping:
                DrawJump(sx, sy, dir, false);
                break;
            case PlayerState.JumpKick:
                DrawJump(sx, sy, dir, true);
                break;
            case PlayerState.Punching:
                DrawPunch(sx, sy, dir);
                break;
            case PlayerState.Kicking:
                DrawKick(sx, sy, dir);
                break;
            case PlayerState.Grabbed:
            case PlayerState.Hurt:
                DrawHurt(sx, sy, dir);
                break;
            case PlayerState.Dead:
                DrawDead(sx, sy);
                break;
        }
    }

    private static void DrawStand(int sx, int sy, int dir, int frame)
    {
        var (fl, bl) = WalkFrames[frame % 4];
        // back leg
        Raylib.DrawRectangle(sx + 4 - (dir < 0 ? 2 : 0), sy + 16 + bl, 4, 8, Pants);
        // front leg
        Raylib.DrawRectangle(sx + 8 - (dir < 0 ? 2 : 0), sy + 16 + fl, 4, 8, Pants);
        // shoes
        Raylib.DrawRectangle(sx + 4, sy + 22 + bl, 5, 2, Shoe);
        Raylib.DrawRectangle(sx + 8, sy + 22 + fl, 5, 2, Shoe);
        DrawUpperBody(sx, sy, dir, 0, false);
    }

    private static void DrawCrouch(int sx, int sy, int dir, PlayerState st)
    {
        // legs bent — draw squatted
        Raylib.DrawRectangle(sx + 3, sy + 18, 5, 5, Pants);
        Raylib.DrawRectangle(sx + 8, sy + 18, 5, 5, Pants);
        Raylib.DrawRectangle(sx + 2, sy + 22, 6, 2, Shoe);
        Raylib.DrawRectangle(sx + 8, sy + 22, 6, 2, Shoe);
        bool attacking = st == PlayerState.CrouchPunch || st == PlayerState.CrouchKick;
        DrawUpperBody(sx, sy + 6, dir, 0, attacking);
        if (st == PlayerState.CrouchKick)
        {
            int ex = dir > 0 ? sx + 16 : sx - 8;
            Raylib.DrawRectangle(ex, sy + 18, 8, 4, Pants);
        }
    }

    private static void DrawJump(int sx, int sy, int dir, bool kick)
    {
        // Legs tucked
        Raylib.DrawRectangle(sx + 3, sy + 14, 4, 6, Pants);
        Raylib.DrawRectangle(sx + 9, sy + 14, 4, 6, Pants);
        DrawUpperBody(sx, sy, dir, -2, kick);
        if (kick)
        {
            int kx = dir > 0 ? sx + 14 : sx - 10;
            Raylib.DrawRectangle(kx, sy + 10, 10, 4, Pants);
        }
    }

    private static void DrawPunch(int sx, int sy, int dir)
    {
        DrawStand(sx, sy, dir, 0);
        // Extend arm
        int ax = dir > 0 ? sx + 14 : sx - 10;
        Raylib.DrawRectangle(ax, sy + 8, 8, 4, Skin);
    }

    private static void DrawKick(int sx, int sy, int dir)
    {
        DrawStand(sx, sy, dir, 0);
        int kx = dir > 0 ? sx + 14 : sx - 10;
        Raylib.DrawRectangle(kx, sy + 14, 10, 4, Pants);
    }

    private static void DrawHurt(int sx, int sy, int dir)
    {
        DrawStand(sx, sy, dir, 0);
        // Red tint overlay
        Raylib.DrawRectangle(sx, sy, 16, 24, HurtTint);
    }

    private static void DrawDead(int sx, int sy)
    {
        // Slumped on ground
        Raylib.DrawRectangle(sx - 4, sy + 18, 24, 6, Pants);
        Raylib.DrawRectangle(sx,     sy + 14, 16, 6, Shirt);
        Raylib.DrawCircle(sx + 8, sy + 12, 4, Skin);
    }

    private static void DrawUpperBody(int sx, int sy, int dir, int yOff, bool armOut)
    {
        // Torso
        Raylib.DrawRectangle(sx + 3, sy + 8 + yOff, 10, 8, Shirt);
        // Belt
        Raylib.DrawRectangle(sx + 3, sy + 14 + yOff, 10, 2, Belt);
        // Head
        Raylib.DrawCircle(sx + 8, sy + 4 + yOff, 4, Skin);
        // Hair
        Raylib.DrawRectangle(sx + 4, sy + 0 + yOff, 8, 3, Hair);
        // Arm (resting)
        int ax = dir > 0 ? sx + 12 : sx + 1;
        Raylib.DrawRectangle(ax, sy + 9 + yOff, 4, 4, Skin);
    }
}
