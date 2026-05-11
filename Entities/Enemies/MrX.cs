using Raylib_cs;
using System.Numerics;
using KungFuClone.Audio;
using KungFuClone.Screens;

namespace KungFuClone.Entities.Enemies;

public class MrX : Enemy
{
    private readonly int _pattern;
    private bool _attacking;
    private bool _highAttack;
    private float _attackCooldown;
    private float _summonCooldown;

    public MrX(float x, int pattern)
    {
        _pattern   = pattern;
        Position.X = x;
        Position.Y = Core.Constants.GroundY - 28f;
        Health = MaxHealth = pattern switch { 5 => 8, 4 => 7, 3 => 6, 2 => 5, _ => 4 };
        ScoreValue   = 5000;
        AttackDamage = 1;
    }

    public override bool IsAttacking => _attacking && StateTimer > 0;
    public override Rectangle AttackHitbox
    {
        get
        {
            if (!IsAttacking) return new(0,0,0,0);
            float ax = FacingRight ? Position.X + 14 : Position.X - 20;
            float ay = _highAttack ? Position.Y + 4 : Position.Y + 14;
            return new(ax, ay, 18, 8);
        }
    }
    public override Rectangle LocalBounds => new(2, 2, 18, 26);

    public override void OnHit(int damage, HitType type)
    {
        Health -= damage;
        AudioEngine.EnemyHit.Play();
        if (Health <= 0) { AudioEngine.EnemyDown.Play(); Active = false; }
        else { EnemyStateValue = EnemyState.Stunned; StateTimer = 0.4f * (1f / _pattern); }
    }

    protected override void UpdateAI(float dt, GameScreen ctx)
    {
        if (Target == null) return;
        if (EnemyStateValue == EnemyState.Stunned) { Velocity.X = 0; return; }

        _attackCooldown -= dt;
        _summonCooldown -= dt;

        float dx = Target.Position.X - Position.X;
        FacingRight = dx > 0;
        float speed = 30f + _pattern * 6f;

        switch (_pattern)
        {
            case 1: Pattern_Punch(dx, speed); break;
            case 2: Pattern_PunchKick(dx, speed); break;
            case 3: Pattern_SummonGrippers(dx, speed, dt, ctx); break;
            case 4: Pattern_Throw(dx, speed, ctx); break;
            case 5: Pattern_All(dx, speed, dt, ctx); break;
        }
    }

    private void Pattern_Punch(float dx, float speed)
    {
        if (MathF.Abs(dx) <= 22f && _attackCooldown <= 0f)
        {
            _attacking   = true;
            _highAttack  = true;
            StateTimer   = 0.4f;
            _attackCooldown = 1.2f;
            Velocity.X   = 0;
        }
        else { _attacking = false; Velocity.X = MathF.Sign(dx) * speed; }
    }

    private void Pattern_PunchKick(float dx, float speed)
    {
        if (MathF.Abs(dx) <= 22f && _attackCooldown <= 0f)
        {
            _attacking  = true;
            _highAttack = (Target!.State != PlayerState.Crouching);
            StateTimer  = 0.35f;
            _attackCooldown = 0.9f;
            Velocity.X  = 0;
        }
        else { _attacking = false; Velocity.X = MathF.Sign(dx) * speed; }
    }

    private void Pattern_SummonGrippers(float dx, float speed, float dt, GameScreen ctx)
    {
        Pattern_PunchKick(dx, speed);
        if (_summonCooldown <= 0f)
        {
            _summonCooldown = 6f;
            ctx.Enemies.Add(new Gripper(Position.X + 60));
            ctx.Enemies.Add(new Gripper(Position.X - 60));
        }
    }

    private void Pattern_Throw(float dx, float speed, GameScreen ctx)
    {
        if (MathF.Abs(dx) <= 30f && _attackCooldown <= 0f)
        {
            _attacking  = true;
            _highAttack = true;
            StateTimer  = 0.3f;
            _attackCooldown = 0.7f;
            // Throw projectile
            var proj = new Projectile
            {
                IsPlayerOwned = false,
                InitialPos    = new Vector2(Position.X, Position.Y + 8),
                TargetPos     = new Vector2(Target!.Position.X, Target.Position.Y + 12),
                ArcHeight     = 10f,
                TravelTime    = 0.5f,
            };
            proj.Position = proj.InitialPos;
            ctx.Projectiles.Add(proj);
            Velocity.X = 0;
        }
        else { _attacking = false; Velocity.X = MathF.Sign(dx) * speed; }
    }

    private void Pattern_All(float dx, float speed, float dt, GameScreen ctx)
    {
        Pattern_Throw(dx, speed, ctx);
        if (_summonCooldown <= 0f)
        {
            _summonCooldown = 8f;
            ctx.Enemies.Add(new Pudgy(Position.X + 80));
        }
    }
}
