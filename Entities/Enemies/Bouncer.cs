using Raylib_cs;
using KungFuClone.Audio;
using KungFuClone.Screens;

namespace KungFuClone.Entities.Enemies;

public class Bouncer : Enemy
{
    private const float Speed = 28f;
    private const float AttackRange = 18f;
    private bool _attacking;

    public Bouncer(float x)
    {
        Position.X = x;
        Position.Y = Core.Constants.GroundY - 28f; // taller
        Health = MaxHealth = 3;
        ScoreValue = 500;
        AttackDamage = 1;
    }

    public override bool IsAttacking => _attacking && StateTimer > 0;
    public override Rectangle AttackHitbox =>
        IsAttacking
            ? new(FacingRight ? Position.X + 12 : Position.X - 14, Position.Y + 6, 16, 10)
            : new(0, 0, 0, 0);
    public override Rectangle LocalBounds => new(2, 4, 16, 24);

    public override void OnHit(int damage, HitType type)
    {
        Health -= damage;
        AudioEngine.EnemyHit.Play();
        if (Health <= 0) { AudioEngine.EnemyDown.Play(); Active = false; }
        else
        {
            // Stagger — brief knockback
            EnemyStateValue = EnemyState.Stunned;
            StateTimer = 0.6f;
            Velocity.X = FacingRight ? -40f : 40f;
            _attacking = false;
        }
    }

    protected override void UpdateAI(float dt, GameScreen ctx)
    {
        if (Target == null) return;
        if (EnemyStateValue == EnemyState.Stunned) { Velocity.X *= 0.8f; return; }

        float dx = Target.Position.X - Position.X;
        FacingRight = dx > 0;

        if (MathF.Abs(dx) <= AttackRange)
        {
            _attacking  = true;
            StateTimer  = 0.4f;
            Velocity.X  = 0;
        }
        else
        {
            _attacking = false;
            Velocity.X = MathF.Sign(dx) * Speed;
        }
    }
}
