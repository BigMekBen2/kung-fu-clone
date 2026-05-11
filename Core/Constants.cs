namespace KungFuClone.Core;

public static class Constants
{
    public const int InternalWidth  = 256;
    public const int InternalHeight = 240;
    public const int Scale          = 3;
    public const int WindowWidth    = InternalWidth  * Scale;
    public const int WindowHeight   = InternalHeight * Scale;

    public const float Gravity      = 480f;   // px/s²
    public const int   GroundY      = 176;    // y of floor surface (top of ground strip)
    public const float ScrollSpeed  = 55f;    // px/s

    public const int   PlayerStartX = 20;
    public const int   PlayerStartY = GroundY - 24;
}
