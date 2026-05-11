using Raylib_cs;

namespace KungFuClone.Audio;

public enum WaveType { Square, Triangle, Noise }

public class SoundEffect
{
    public WaveType Type        = WaveType.Square;
    public float    FreqStart   = 440f;
    public float    FreqEnd     = 440f;    // sweep target
    public float    Duration    = 0.1f;
    public float    Volume      = 0.6f;
    public float    DutyCycle   = 0.5f;
    public float    AttackTime  = 0.005f;
    public float    ReleaseTime = 0.02f;

    private const int SampleRate = 44100;
    private Sound _sound;
    private bool  _baked;

    public void EnsureBaked()
    {
        if (_baked) return;
        _baked = true;

        int totalSamples = (int)(SampleRate * Duration);
        short[] data = new short[totalSamples];
        uint noiseSeed = 12345u;
        float phase = 0f;

        for (int i = 0; i < totalSamples; i++)
        {
            float t    = i / (float)totalSamples;
            float freq = FreqStart + (FreqEnd - FreqStart) * t;

            float sample = Type switch
            {
                WaveType.Square   => Waveforms.Square(phase, DutyCycle),
                WaveType.Triangle => Waveforms.Triangle(phase),
                WaveType.Noise    => Waveforms.Noise(ref noiseSeed),
                _                 => 0f
            };

            // Envelope
            float envelope = 1f;
            float atSamples = AttackTime * SampleRate;
            float relSamples = ReleaseTime * SampleRate;
            if (i < atSamples)
                envelope = i / atSamples;
            else if (i > totalSamples - relSamples)
                envelope = (totalSamples - i) / relSamples;

            data[i] = (short)(sample * envelope * Volume * short.MaxValue);

            phase += freq / SampleRate;
            if (phase > 1f) phase -= 1f;
        }

        unsafe
        {
            fixed (short* ptr = data)
            {
                Wave wave = new Wave
                {
                    SampleCount = (uint)totalSamples,
                    SampleRate  = (uint)SampleRate,
                    SampleSize  = 16,
                    Channels    = 1,
                    Data        = ptr
                };
                _sound = Raylib.LoadSoundFromWave(wave);
            }
        }
    }

    public void Play()
    {
        EnsureBaked();
        Raylib.PlaySound(_sound);
    }

    public void Unload()
    {
        if (_baked) Raylib.UnloadSound(_sound);
    }
}
