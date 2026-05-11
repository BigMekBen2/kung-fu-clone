using Raylib_cs;
using System.Numerics;
using KungFuClone.Audio;
using KungFuClone.Screens;

namespace KungFuClone.Entities.Enemies;

public class KnifeThrower : Enemy
{
    private const float PreferredDist = 90f;
    private const float ThrowCooldown = 2.2f;
    private float _throwTimer = 1f;

    public KnifeThrower(float x)
    {
        Position.X = x;
        Position.Y = Core.Constants.GroundY - 24f;
        Health = MaxHealth = 1;
        ScoreValue = 200;
        AttackDamage = 1;
    }

    public override bool IsAttacking => false; // uses projectiles
    public override Rectangle AttackHitbox => new(0, 0, 0, 0);
    public override Rectangle LocalBounds  => new(2, 4, 12, 20);

    public override void OnHit(int damage, HitType type)
    {
        Health -= damage;
        AudioEngine.EnemyHit.Play();
        if (Health <= 0) { AudioEngine.EnemyDown.Play(); Active = false; }
        else { EnemyStateValue = EnemyState.Stunned; StateTimer = 0.4f; }
    }

    protected override void UpdateAI(float dt, GameScreen ctx)
    {
        if (Target == null) return;
        float dx = Target.Position.X - Position.X;
        FacingRight = dx > 0;

        if (EnemyStateValue == EnemyState.Stunned) { Velocity.X = 0; return; }

        // Maintain preferred distance
        float absDx = MathF.Abs(dx);
        if (absDx > PreferredDist + 20) Velocity.X = MathF.Sign(dx) * 35f;
        else if (absDx < PreferredDist - 20) Velocity.X = -MathF.Sign(dx) * 25f;
        else Velocity.X = 0;

        _throwTimer -= dt;
        if (_throwTimer <= 0f)
        {
            _throwTimer = ThrowCooldown;
            ThrowKnife(ctx);
        }
    }

    private void ThrowKnife(GameScreen ctx)
    {
        if (Target == null) return;
        AudioEngine.KnifeThrow.Play();
        var proj = new Projectile
        {
            IsPlayerOwned = false,
            InitialPos    = new Vector2(Position.X + (FacingRight ? 8 : -4), Position.Y + 8),
            TargetPos     = new Vector2(Target.Position.X, Target.Position.Y + 12),
            ArcHeight     = 18f,
            TravelTime    = MathF.Abs(Target.Position.X - Position.X) / 110f + 0.3f,
        };
        proj.Position = proj.InitialPos;
        ctx.Projectiles.Add(proj);
    }
}
