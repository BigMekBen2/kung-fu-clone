using Raylib_cs;
using KungFuClone.Audio;
using KungFuClone.Screens;

namespace KungFuClone.Entities.Enemies;

// Gripper/Bouncer hybrid — faster, more aggressive
public class Pudgy : Enemy
{
    private const float Speed     = 55f;
    private const float GrabRange = 14f;
    private bool _grabbing;

    public Pudgy(float x)
    {
        Position.X = x;
        Position.Y = Core.Constants.GroundY - 24f;
        Health = MaxHealth = 2;
        ScoreValue = 400;
        AttackDamage = 1;
    }

    public override bool IsAttacking => _grabbing;
    public override Rectangle AttackHitbox => _grabbing
        ? new(Position.X, Position.Y + 4, 14, 16) : new(0,0,0,0);
    public override Rectangle LocalBounds => new(2, 4, 12, 20);

    public override void OnHit(int damage, HitType type)
    {
        Health -= damage;
        AudioEngine.EnemyHit.Play();
        if (Health <= 0) { AudioEngine.EnemyDown.Play(); Active = false; }
        else { EnemyStateValue = EnemyState.Stunned; StateTimer = 0.35f; Velocity.X = 0; }
    }

    protected override void UpdateAI(float dt, GameScreen ctx)
    {
        if (Target == null) return;
        if (EnemyStateValue == EnemyState.Stunned) { Velocity.X = 0; return; }

        float dx = Target.Position.X - Position.X;
        FacingRight = dx > 0;

        if (MathF.Abs(dx) <= GrabRange && !Target.IsGrabbed && !Target.IsInvincible)
        {
            _grabbing = true;
            Target.IsGrabbed = true;
            Target.MashMeter = 0f;
            AudioEngine.Grab.Play();
            Velocity.X = 0;
        }
        else if (!_grabbing)
        {
            Velocity.X = MathF.Sign(dx) * Speed;
        }

        if (_grabbing && Target.IsGrabbed)
            Position.X = Target.Position.X + (FacingRight ? -GrabRange : GrabRange);
        else if (_grabbing && !Target.IsGrabbed)
        {
            _grabbing = false;
            EnemyStateValue = EnemyState.Stunned;
            StateTimer = 0.4f;
        }
    }
}
