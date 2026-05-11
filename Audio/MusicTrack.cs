using Raylib_cs;

namespace KungFuClone.Audio;

public class MusicTrack
{
    private const int SampleRate   = 44100;
    private const int BufferFrames = 2048;

    private AudioStream _stream;
    private short[]     _buffer   = new short[BufferFrames];

    // Sequencer state
    private (float freq, float beats)[] _pattern = Array.Empty<(float, float)>();
    private int   _noteIndex;
    private float _notePhase;
    private float _noteSamples;   // samples remaining in current note
    private float _bpm           = 160f;
    private bool  _initialized;

    // Floor BGM patterns (freq Hz, beats) — approximation of Kung Fu Master BGM
    private static readonly (float freq, float beats)[] Floor1Pattern =
    {
        (330,1),(330,0.5f),(330,0.5f),(392,1),(330,1),
        (294,1),(262,1),(294,0.5f),(330,0.5f),(392,1),(440,1),
        (330,1),(330,0.5f),(330,0.5f),(392,1),(330,1),
        (262,2),(0,1),(262,1),(294,1),(330,1),(392,1),
    };

    private static readonly (float freq, float beats)[] Floor2Pattern =
    {
        (440,1),(494,0.5f),(440,0.5f),(392,1),(349,1),
        (330,1),(294,0.5f),(262,0.5f),(294,1),(330,1),
        (392,1),(440,1),(494,0.5f),(523,0.5f),(494,1),(440,1),
        (392,2),(0,0.5f),(349,0.5f),(330,1),(294,1),
    };

    private static readonly (float freq, float beats)[] Floor3Pattern =
    {
        (523,0.5f),(494,0.5f),(440,1),(392,1),(349,1),(330,1),
        (294,0.5f),(330,0.5f),(349,1),(392,1),(440,1),(494,1),
        (523,0.5f),(494,0.5f),(440,1),(0,0.5f),(440,0.5f),(494,1),
        (523,2),(0,0.5f),(392,0.5f),(440,1),(494,1),
    };

    private static readonly (float freq, float beats)[][] AllPatterns =
        { Floor1Pattern, Floor2Pattern, Floor3Pattern, Floor2Pattern, Floor3Pattern };

    public void Init(int floor)
    {
        if (_initialized) Unload();
        _pattern    = AllPatterns[Math.Clamp(floor - 1, 0, AllPatterns.Length - 1)];
        _noteIndex  = 0;
        _notePhase  = 0f;
        _bpm        = 140f + floor * 8f;
        StartNote();

        _stream = Raylib.LoadAudioStream((uint)SampleRate, 16, 1);
        Raylib.PlayAudioStream(_stream);
        _initialized = true;
    }

    public void Update()
    {
        if (!_initialized) return;
        if (!Raylib.IsAudioStreamProcessed(_stream)) return;
        FillBuffer();
        unsafe
        {
            fixed (short* ptr = _buffer)
                Raylib.UpdateAudioStream(_stream, ptr, BufferFrames);
        }
    }

    private void FillBuffer()
    {
        float samplesPerBeat = SampleRate * (60f / _bpm);

        for (int i = 0; i < BufferFrames; i++)
        {
            float freq = _pattern[_noteIndex].freq;
            float sample = freq > 0 ? Waveforms.Square(_notePhase, 0.5f) * 0.3f : 0f;
            _buffer[i] = (short)(sample * short.MaxValue);

            if (freq > 0) _notePhase += freq / SampleRate;
            if (_notePhase > 1f) _notePhase -= 1f;

            _noteSamples--;
            if (_noteSamples <= 0f)
            {
                _noteIndex = (_noteIndex + 1) % _pattern.Length;
                StartNote();
            }
        }
    }

    private void StartNote()
    {
        float samplesPerBeat = SampleRate * (60f / _bpm);
        _noteSamples = samplesPerBeat * _pattern[_noteIndex].beats;
        _notePhase   = 0f;
    }

    public void Unload()
    {
        if (_initialized) { Raylib.StopAudioStream(_stream); Raylib.UnloadAudioStream(_stream); }
        _initialized = false;
    }
}
