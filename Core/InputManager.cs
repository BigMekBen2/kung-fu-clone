using Raylib_cs;

namespace KungFuClone.Core;

public class InputManager
{
    // Virtual NES buttons
    public bool Left       => Raylib.IsKeyDown(KeyboardKey.Left);
    public bool Right      => Raylib.IsKeyDown(KeyboardKey.Right);
    public bool Up         => Raylib.IsKeyDown(KeyboardKey.Up);
    public bool Down       => Raylib.IsKeyDown(KeyboardKey.Down);
    public bool Punch      => Raylib.IsKeyDown(KeyboardKey.X);
    public bool Kick       => Raylib.IsKeyDown(KeyboardKey.Z);
    public bool PunchPress => Raylib.IsKeyPressed(KeyboardKey.X);
    public bool KickPress  => Raylib.IsKeyPressed(KeyboardKey.Z);
    public bool Start      => Raylib.IsKeyPressed(KeyboardKey.Enter);
    public bool Select     => Raylib.IsKeyPressed(KeyboardKey.RightShift);

    // Any key pressed this frame (for mash detection)
    public bool AnyPressed =>
        Raylib.IsKeyPressed(KeyboardKey.X)     ||
        Raylib.IsKeyPressed(KeyboardKey.Z)     ||
        Raylib.IsKeyPressed(KeyboardKey.Left)  ||
        Raylib.IsKeyPressed(KeyboardKey.Right) ||
        Raylib.IsKeyPressed(KeyboardKey.Up)    ||
        Raylib.IsKeyPressed(KeyboardKey.Down);
}
