using Raylib_cs;
using KungFuClone.Audio;
using KungFuClone.Screens;

namespace KungFuClone.Entities.Enemies;

public class StickFighter : Enemy
{
    private const float Speed       = 38f;
    private const float AttackRange = 22f;
    private bool  _attacking;
    private bool  _highAttack;   // true=high, false=low
    private float _attackCooldown;

    public StickFighter(float x)
    {
        Position.X = x;
        Position.Y = Core.Constants.GroundY - 24f;
        Health = MaxHealth = 2;
        ScoreValue = 300;
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
            return new(ax, ay, 18, 6);
        }
    }
    public override Rectangle LocalBounds => new(2, 4, 12, 20);

    public override void OnHit(int damage, HitType type)
    {
        // Block based on attack type vs stance
        bool blocked = (_highAttack  && type == HitType.LowPunch)
                    || (!_highAttack && type == HitType.Punch);
        if (blocked) return;

        Health -= damage;
        AudioEngine.EnemyHit.Play();
        if (Health <= 0) { AudioEngine.EnemyDown.Play(); Active = false; }
        else { EnemyStateValue = EnemyState.Stunned; StateTimer = 0.5f; _attacking = false; }
    }

    protected override void UpdateAI(float dt, GameScreen ctx)
    {
        if (Target == null) return;
        if (EnemyStateValue == EnemyState.Stunned) { Velocity.X = 0; return; }

        _attackCooldown -= dt;
        float dx = Target.Position.X - Position.X;
        FacingRight = dx > 0;

        if (MathF.Abs(dx) <= AttackRange && _attackCooldown <= 0f)
        {
            _attacking    = true;
            _highAttack   = (Target.State != Entities.PlayerState.Crouching);
            StateTimer    = 0.35f;
            _attackCooldown = 1.4f;
            Velocity.X    = 0;
        }
        else
        {
            _attacking = false;
            Velocity.X = MathF.Sign(dx) * Speed;
        }
    }
}
