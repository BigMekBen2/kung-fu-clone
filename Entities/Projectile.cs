using System.Numerics;
using Raylib_cs;

namespace KungFuClone.Entities;

public class Projectile : Entity
{
    public bool    IsPlayerOwned = false;
    public Vector2 InitialPos;
    public float   ArcHeight    = 20f;
    public float   TravelTime   = 1.5f;
    public float   Elapsed      = 0f;
    public Vector2 TargetPos;

    public override Rectangle LocalBounds => new(-2, -2, 4, 4);

    public void Update(float dt)
    {
        Elapsed += dt;
        if (Elapsed >= TravelTime) { Active = false; return; }

        float t = Elapsed / TravelTime;
        Position.X = InitialPos.X + (TargetPos.X - InitialPos.X) * t;
        float baseY = InitialPos.Y + (TargetPos.Y - InitialPos.Y) * t;
        Position.Y = baseY - MathF.Sin(t * MathF.PI) * ArcHeight;
    }

    public void Draw(float cameraX)
    {
        if (!Active) return;
        int sx = (int)(Position.X - cameraX);
        int sy = (int)Position.Y;
        // Knife: small orange rectangle
        Raylib.DrawRectangle(sx - 3, sy - 1, 6, 2, new Color(0xFF, 0xA0, 0x00, 255));
    }
}
