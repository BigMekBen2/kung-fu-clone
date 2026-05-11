using Raylib_cs;
using KungFuClone.Entities;
using KungFuClone.Screens;

namespace KungFuClone.Entities.Enemies;

public enum EnemyState { Idle, Walking, Attacking, Stunned, Dead }

public abstract class Enemy : Entity
{
    public int         Health;
    public int         MaxHealth;
    public int         ScoreValue;
    public EnemyState  EnemyStateValue  = EnemyState.Idle;
    public float       StateTimer       = 0f;
    public int         AttackDamage     = 1;
    public bool        ScoreAwarded     = false;
    public float       HitCooldown      = 0f;   // invincibility after a hit
    public bool        CanBeHit         => HitCooldown <= 0f;

    public abstract bool      IsAttacking  { get; }
    public abstract Rectangle AttackHitbox { get; }

    protected Player? Target;

    public abstract void OnHit(int damage, HitType type);
    protected abstract void UpdateAI(float dt, GameScreen ctx);

    public void Update(float dt, GameScreen ctx)
    {
        if (!Active) return;
        Target = ctx.Player;
        StateTimer  -= dt;
        HitCooldown -= dt;
        UpdateAI(dt, ctx);
        Position.X += Velocity.X * dt;
        Position.Y += Velocity.Y * dt;

        // Ground clamp
        float groundTop = Core.Constants.GroundY - 24f;
        if (Position.Y > groundTop) Position.Y = groundTop;
    }
}
