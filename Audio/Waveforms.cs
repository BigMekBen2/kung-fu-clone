namespace KungFuClone.Audio;

public static class Waveforms
{
    public static float Square(float phase, float duty = 0.5f)
        => (phase % 1f) < duty ? 1f : -1f;

    public static float Triangle(float phase)
    {
        float t = phase % 1f;
        return t < 0.5f ? (4f * t - 1f) : (3f - 4f * t);
    }

    public static float Noise(ref uint seed)
    {
        seed ^= seed << 13; seed ^= seed >> 17; seed ^= seed << 5;
        return (seed / (float)uint.MaxValue) * 2f - 1f;
    }
}
