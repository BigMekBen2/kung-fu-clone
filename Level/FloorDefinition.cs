namespace KungFuClone.Level;

public enum EnemyType { Gripper, KnifeThrower, StickFighter, Bouncer, Pudgy, MrX }

public class SpawnEntry
{
    public EnemyType Type;
    public float     WorldX;
    public int       Side    = 1;   // +1 = right edge, -1 = left edge spawn
}

public class BossSpawn
{
    public float WorldX;
    public int   AttackPattern;   // 1–5
}

public class FloorDefinition
{
    public int          FloorNumber;
    public float        TotalWidth;
    public int          BackgroundTheme;
    public float        TimeLimit;
    public SpawnEntry[] Spawns  = Array.Empty<SpawnEntry>();
    public BossSpawn?   Boss;
}
