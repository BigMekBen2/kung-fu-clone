using KungFuClone.Entities;
using KungFuClone.Entities.Enemies;

namespace KungFuClone.Systems;

public class CollisionSystem
{
    public void Resolve(Player player, List<Enemy> enemies, List<Projectile> projectiles, ScoreSystem score)
    {
        // Player attacks enemies
        if (player.IsAttacking)
        {
            var hitbox = player.AttackHitbox;
            foreach (var e in enemies)
            {
                if (!e.Active || !e.CanBeHit) continue;
                if (CheckOverlap(hitbox, e.WorldBounds))
                {
                    e.HitCooldown = 0.3f;   // one hit per swing
                    e.OnHit((int)Player.AttackDamage, player.CurrentHitType);
                }
            }
        }

        // Enemies attack player
        if (!player.IsInvincible)
        {
            foreach (var e in enemies)
            {
                if (!e.Active || !e.IsAttacking) continue;
                if (CheckOverlap(e.AttackHitbox, player.WorldBounds))
                    player.TakeDamage(e.AttackDamage);
            }
        }

        // Enemy projectiles hit player
        if (!player.IsInvincible)
        {
            foreach (var p in projectiles)
            {
                if (!p.Active || p.IsPlayerOwned) continue;
                if (CheckOverlap(p.WorldBounds, player.WorldBounds))
                {
                    player.TakeDamage(1);
                    p.Active = false;
                }
            }
        }

        // Score dead enemies
        foreach (var e in enemies)
        {
            if (!e.Active && !e.ScoreAwarded)
            {
                score.Add(e.ScoreValue);
                e.ScoreAwarded = true;
            }
        }

        enemies.RemoveAll(e => !e.Active);
        projectiles.RemoveAll(p => !p.Active);
    }

    private static bool CheckOverlap(Raylib_cs.Rectangle a, Raylib_cs.Rectangle b)
        => a.X < b.X + b.Width  && a.X + a.Width  > b.X
        && a.Y < b.Y + b.Height && a.Y + a.Height > b.Y;
}
