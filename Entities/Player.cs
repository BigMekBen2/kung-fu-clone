using System.Numerics;
using Raylib_cs;
using KungFuClone.Audio;
using KungFuClone.Core;

namespace KungFuClone.Entities;

public enum PlayerState
{
    Idle, Walking, Crouching,
    Jumping, JumpKick,
    Punching, Kicking,
    CrouchPunch, CrouchKick,
    Grabbed, Hurt, Dead
}

public class Player : Entity
{
    public PlayerState State       = PlayerState.Idle;
    public int         Health      = 3;
    public int         Lives       = 3;
    public float       StateTimer  = 0f;        // time remaining in timed state
    public float       InvincibleTimer = 0f;
    public bool        IsOnGround  = true;
    public bool        IsGrabbed   = false;
    public float       MashMeter   = 0f;

    private const float AttackDuration   = 0.25f;
    private const float HurtDuration     = 0.4f;
    private const float InvincibleTime   = 1.2f;
    private const float JumpSpeed        = -260f;
    public  const float AttackDamage     = 1f;
    private const float MashThreshold   = 8f;
    private const float GrabTimeout     = 2f;
    private float _grabTimer            = 0f;

    public bool IsAttacking => State is PlayerState.Punching or PlayerState.Kicking
        or PlayerState.CrouchPunch or PlayerState.CrouchKick or PlayerState.JumpKick
        && StateTimer > AttackDuration * 0.3f;  // active window is first 70%

    public bool IsInvincible => InvincibleTimer > 0f;

    public HitType CurrentHitType => State switch
    {
        PlayerState.Punching    => HitType.Punch,
        PlayerState.CrouchPunch => HitType.LowPunch,
        PlayerState.Kicking     => HitType.Kick,
        PlayerState.CrouchKick  => HitType.LowKick,
        PlayerState.JumpKick    => HitType.JumpKick,
        _                       => HitType.Punch
    };

    public override Rectangle LocalBounds => new(2, 4, 12, 20);

    public Rectangle AttackHitbox
    {
        get
        {
            float cx = Position.X + (FacingRight ? 14 : -10);
            return State switch
            {
                PlayerState.Punching or PlayerState.CrouchPunch =>
                    new Rectangle(cx, Position.Y + 6, 10, 6),
                PlayerState.Kicking  or PlayerState.CrouchKick =>
                    new Rectangle(cx, Position.Y + 14, 12, 6),
                PlayerState.JumpKick =>
                    new Rectangle(cx, Position.Y + 8, 12, 8),
                _ => new Rectangle(0, 0, 0, 0)
            };
        }
    }

    public void Update(float dt, InputManager input)
    {
        InvincibleTimer -= dt;

        if (IsGrabbed)
        {
            HandleGrabbed(dt, input);
            return;
        }

        if (StateTimer > 0f)
        {
            StateTimer -= dt;
            if (StateTimer <= 0f)
                ExitTimedState();
            ApplyGravity(dt);
            ClampToGround();
            return;
        }

        if (State == PlayerState.Hurt || State == PlayerState.Dead)
        {
            ApplyGravity(dt);
            ClampToGround();
            return;
        }

        HandleMovement(dt, input);
        HandleAttack(input);
        ApplyGravity(dt);
        ClampToGround();
    }

    private void HandleMovement(float dt, InputManager input)
    {
        bool crouching = input.Down && IsOnGround;

        if (crouching)
        {
            State = PlayerState.Crouching;
            Velocity.X = 0f;
            return;
        }

        if (input.Left)  { Velocity.X = -70f; FacingRight = false; }
        else if (input.Right) { Velocity.X =  70f; FacingRight = true; }
        else Velocity.X = 0f;

        if (input.Up && IsOnGround)
        {
            Velocity.Y = JumpSpeed;
            IsOnGround = false;
            State = PlayerState.Jumping;
            AudioEngine.Jump.Play();
            return;
        }

        State = Velocity.X != 0f ? PlayerState.Walking : PlayerState.Idle;
        if (!IsOnGround) State = PlayerState.Jumping;
    }

    private void HandleAttack(InputManager input)
    {
        bool crouching = State == PlayerState.Crouching;

        if (input.PunchPress)
        {
            State      = crouching ? PlayerState.CrouchPunch : PlayerState.Punching;
            StateTimer = AttackDuration;
            AudioEngine.Punch.Play();
        }
        else if (input.KickPress)
        {
            if (!IsOnGround)
            {
                State      = PlayerState.JumpKick;
                StateTimer = AttackDuration;
            }
            else
            {
                State      = crouching ? PlayerState.CrouchKick : PlayerState.Kicking;
                StateTimer = AttackDuration;
            }
            AudioEngine.Kick.Play();
        }
    }

    private void HandleGrabbed(float dt, InputManager input)
    {
        _grabTimer += dt;
        if (input.AnyPressed) MashMeter++;
        if (MashMeter >= MashThreshold)
        {
            Escape();
        }
        else if (_grabTimer >= GrabTimeout)
        {
            TakeDamage(1);
            Escape();
        }
    }

    public void Escape()
    {
        IsGrabbed  = false;
        MashMeter  = 0f;
        _grabTimer = 0f;
        State = PlayerState.Idle;
    }

    private void ExitTimedState()
    {
        State = IsOnGround ? PlayerState.Idle : PlayerState.Jumping;
    }

    public void TakeDamage(int amount)
    {
        if (IsInvincible) return;
        AudioEngine.Hurt.Play();
        Health -= amount;
        InvincibleTimer = InvincibleTime;
        if (Health <= 0)
        {
            Health = 0;
            State  = PlayerState.Dead;
        }
        else
        {
            State      = PlayerState.Hurt;
            StateTimer = HurtDuration;
            Velocity.Y = -60f;
            IsOnGround = false;
        }
    }

    private void ApplyGravity(float dt)
    {
        if (!IsOnGround)
            Velocity.Y += Constants.Gravity * dt;
        Position += Velocity * dt;
    }

    private void ClampToGround()
    {
        float groundTop = Constants.GroundY - 24f; // player height
        if (Position.Y >= groundTop)
        {
            Position.Y = groundTop;
            Velocity.Y = 0f;
            bool wasAirborne = !IsOnGround;
            IsOnGround = true;
            if (wasAirborne && State is PlayerState.Jumping or PlayerState.JumpKick
                            or PlayerState.Hurt)
                State = PlayerState.Idle;
        }
    }
}

public enum HitType { Punch, LowPunch, Kick, LowKick, JumpKick }
