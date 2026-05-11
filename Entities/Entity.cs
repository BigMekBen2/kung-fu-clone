using System.Numerics;
using Raylib_cs;

namespace KungFuClone.Entities;

public abstract class Entity
{
    public Vector2  Position;
    public Vector2  Velocity;
    public bool     Active = true;
    public bool     FacingRight = true;

    // Local bounding box (relative to Position)
    public abstract Rectangle LocalBounds { get; }

    public Rectangle WorldBounds => new(
        Position.X + LocalBounds.X,
        Position.Y + LocalBounds.Y,
        LocalBounds.Width,
        LocalBounds.Height);
}
