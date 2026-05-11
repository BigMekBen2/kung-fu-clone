using KungFuClone.Core;
using KungFuClone.Entities;
using KungFuClone.Entities.Enemies;
using KungFuClone.Level;

namespace KungFuClone.Systems;

public class SpawnSystem
{
    private Queue<SpawnEntry> _pending = new();
    private BossSpawn?        _boss;
    private bool              _bossSpawned;
    private int               _direction = 1;   // +1 right, -1 left

    public void LoadFloor(FloorDefinition floor)
    {
        _direction   = floor.Direction;
        _bossSpawned = false;
        _boss        = floor.Boss;

        // Right-scroll: trigger ascending (low WorldX first); left-scroll: descending
        var sorted = _direction >= 0
            ? floor.Spawns.OrderBy(s => s.WorldX)
            : floor.Spawns.OrderByDescending(s => s.WorldX);
        _pending = new Queue<SpawnEntry>(sorted);
    }

    public void Update(float cameraX, List<Enemy> enemies)
    {
        float rightEdge = cameraX + Constants.InternalWidth;
        float leftEdge  = cameraX;

        while (_pending.Count > 0)
        {
            var next = _pending.Peek();
            bool trigger = _direction >= 0
                ? rightEdge >= next.WorldX          // approaching from left
                : leftEdge  <= next.WorldX;         // approaching from right (moving left)
            if (!trigger) break;
            _pending.Dequeue();

            // Spawn off the leading edge
            float spawnX = _direction >= 0
                ? cameraX + Constants.InternalWidth + 4
                : cameraX - 20;
            enemies.Add(CreateEnemy(next.Type, spawnX));
        }

        // Boss
        if (!_bossSpawned && _boss != null)
        {
            bool trigger = _direction >= 0
                ? rightEdge >= _boss.WorldX
                : leftEdge  <= _boss.WorldX;
            if (trigger)
            {
                _bossSpawned = true;
                float spawnX = _direction >= 0
                    ? cameraX + Constants.InternalWidth + 4
                    : cameraX - 20;
                enemies.Add(new MrX(spawnX, _boss.AttackPattern));
            }
        }
    }

    private static Enemy CreateEnemy(EnemyType type, float x) => type switch
    {
        EnemyType.Gripper      => new Gripper(x),
        EnemyType.KnifeThrower => new KnifeThrower(x),
        EnemyType.StickFighter => new StickFighter(x),
        EnemyType.Bouncer      => new Bouncer(x),
        EnemyType.Pudgy        => new Pudgy(x),
        EnemyType.MrX          => new MrX(x, 1),
        _                      => new Gripper(x)
    };
}
