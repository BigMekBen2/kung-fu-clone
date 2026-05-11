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
    private float             _floorWidth;

    public void LoadFloor(FloorDefinition floor)
    {
        _pending     = new Queue<SpawnEntry>(floor.Spawns.OrderBy(s => s.WorldX));
        _boss        = floor.Boss;
        _bossSpawned = false;
        _floorWidth  = floor.TotalWidth;
    }

    public void Update(float cameraX, List<Enemy> enemies, List<Enemy> allEnemies)
    {
        float rightEdge = cameraX + Constants.InternalWidth;

        while (_pending.Count > 0 && rightEdge >= _pending.Peek().WorldX)
        {
            var entry = _pending.Dequeue();
            float spawnX = entry.Side >= 0
                ? cameraX + Constants.InternalWidth + 4
                : cameraX - 20;
            enemies.Add(CreateEnemy(entry.Type, spawnX));
        }

        // Boss spawn
        if (!_bossSpawned && _boss != null && rightEdge >= _boss.WorldX)
        {
            _bossSpawned = true;
            float spawnX = cameraX + Constants.InternalWidth + 4;
            enemies.Add(new MrX(spawnX, _boss.AttackPattern));
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
