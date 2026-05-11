using KungFuClone.Core;

namespace KungFuClone.Systems;

public class ScoreSystem
{
    private readonly GameContext _ctx;
    private const int ExtraLifeThreshold = 50000;
    private int _nextExtraLife;

    public ScoreSystem(GameContext ctx)
    {
        _ctx = ctx;
        _nextExtraLife = ExtraLifeThreshold;
    }

    public void Add(int points)
    {
        _ctx.Score += points;
        if (_ctx.Score > _ctx.HiScore)
            _ctx.HiScore = _ctx.Score;

        if (_ctx.Score >= _nextExtraLife)
        {
            _ctx.Lives++;
            _nextExtraLife += ExtraLifeThreshold;
        }
    }
}
