using Raylib_cs;
using KungFuClone.Audio;
using KungFuClone.Entities;
using KungFuClone.Screens;

namespace KungFuClone.Entities.Enemies;

public class Gripper : Enemy
{
    private const float Speed        = 42f;
    private const float GrabRange    = 12f;
    private const float AttackWindow = 0.3f;

    private bool _grabbing;

    public Gripper(float x)
    {
        Position.X  = x;
        Position.Y  = Core.Constants.GroundY - 24f;
        Health      = 1;
        MaxHealth   = 1;
        ScoreValue  = 100;
        AttackDamage = 1;
    }

    public override bool IsAttacking => _grabbing;

    public override Rectangle AttackHitbox => _grabbing
        ? new Rectangle(Position.X, Position.Y + 4, 14, 16)
        : new Rectangle(0, 0, 0, 0);

    public override Rectangle LocalBounds => new(2, 4, 12, 20);

    public override void OnHit(int damage, HitType type)
    {
        Health -= damage;
        AudioEngine.EnemyHit.Play();
        if (Health <= 0)
        {
            AudioEngine.EnemyDown.Play();
            Active = false;
        }
        else
        {
            EnemyStateValue = EnemyState.Stunned;
            StateTimer = 0.4f;
            Velocity.X = 0;
        }
    }

    protected override void UpdateAI(float dt, GameScreen ctx)
    {
        if (Target == null || EnemyStateValue == EnemyState.Dead) return;

        if (Target.IsGrabbed && Target.MashMeter >= 0f)
        {
            // If target escaped us, stop grabbing
            if (!Target.IsGrabbed) _grabbing = false;
        }

        if (EnemyStateValue == EnemyState.Stunned)
        {
            Velocity.X = 0;
            if (StateTimer <= 0f) EnemyStateValue = EnemyState.Walking;
            return;
        }

        float dx = Target.Position.X - Position.X;
        FacingRight = dx > 0;

        if (MathF.Abs(dx) <= GrabRange && !Target.IsGrabbed && !Target.IsInvincible)
        {
            // Grab!
            _grabbing = true;
            Target.IsGrabbed = true;
            Target.MashMeter = 0f;
            AudioEngine.Grab.Play();
            Velocity.X = 0;
            EnemyStateValue = EnemyState.Attacking;
        }
        else if (!_grabbing)
        {
            Velocity.X = MathF.Sign(dx) * Speed;
            EnemyStateValue = EnemyState.Walking;
        }

        // Keep position locked to player while grabbing
        if (_grabbing && Target.IsGrabbed)
        {
            Position.X = Target.Position.X + (FacingRight ? -GrabRange : GrabRange);
        }
        else if (_grabbing && !Target.IsGrabbed)
        {
            _grabbing = false;
            EnemyStateValue = EnemyState.Stunned;
            StateTimer = 0.5f;
        }
    }
}
