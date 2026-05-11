namespace KungFuClone.Audio;

public class AudioEngine
{
    // SFX definitions
    public static readonly SoundEffect Punch = new()
    {
        Type = WaveType.Square, FreqStart = 220f, FreqEnd = 110f,
        Duration = 0.08f, Volume = 0.7f, DutyCycle = 0.5f, ReleaseTime = 0.04f
    };
    public static readonly SoundEffect Kick = new()
    {
        Type = WaveType.Square, FreqStart = 180f, FreqEnd = 80f,
        Duration = 0.10f, Volume = 0.7f, DutyCycle = 0.4f, ReleaseTime = 0.05f
    };
    public static readonly SoundEffect Hurt = new()
    {
        Type = WaveType.Noise, FreqStart = 440f, FreqEnd = 440f,
        Duration = 0.15f, Volume = 0.8f
    };
    public static readonly SoundEffect EnemyHit = new()
    {
        Type = WaveType.Square, FreqStart = 300f, FreqEnd = 150f,
        Duration = 0.06f, Volume = 0.5f
    };
    public static readonly SoundEffect EnemyDown = new()
    {
        Type = WaveType.Noise, FreqStart = 200f, FreqEnd = 200f,
        Duration = 0.2f, Volume = 0.6f, ReleaseTime = 0.1f
    };
    public static readonly SoundEffect Grab = new()
    {
        Type = WaveType.Triangle, FreqStart = 150f, FreqEnd = 100f,
        Duration = 0.18f, Volume = 0.6f
    };
    public static readonly SoundEffect KnifeThrow = new()
    {
        Type = WaveType.Square, FreqStart = 800f, FreqEnd = 400f,
        Duration = 0.06f, Volume = 0.4f, DutyCycle = 0.25f
    };
    public static readonly SoundEffect BonusBag = new()
    {
        Type = WaveType.Triangle, FreqStart = 660f, FreqEnd = 880f,
        Duration = 0.12f, Volume = 0.5f
    };
    public static readonly SoundEffect GameOverSfx = new()
    {
        Type = WaveType.Square, FreqStart = 440f, FreqEnd = 110f,
        Duration = 0.8f, Volume = 0.8f, ReleaseTime = 0.3f
    };
    public static readonly SoundEffect Jump = new()
    {
        Type = WaveType.Square, FreqStart = 300f, FreqEnd = 600f,
        Duration = 0.07f, Volume = 0.4f
    };

    public void BakeAll()
    {
        Punch.EnsureBaked();
        Kick.EnsureBaked();
        Hurt.EnsureBaked();
        EnemyHit.EnsureBaked();
        EnemyDown.EnsureBaked();
        Grab.EnsureBaked();
        KnifeThrow.EnsureBaked();
        BonusBag.EnsureBaked();
        GameOverSfx.EnsureBaked();
        Jump.EnsureBaked();
    }

    public void UnloadAll()
    {
        Punch.Unload(); Kick.Unload(); Hurt.Unload();
        EnemyHit.Unload(); EnemyDown.Unload(); Grab.Unload();
        KnifeThrow.Unload(); BonusBag.Unload(); GameOverSfx.Unload();
        Jump.Unload();
    }
}
