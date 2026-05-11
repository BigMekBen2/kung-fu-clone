namespace KungFuClone.Core;

public enum GameStateId { Title, Playing, BonusStage, GameOver }

public class GameContext
{
    private const string HiScoreFile = "hiscore.dat";

    public int   Score            = 0;
    public int   HiScore          = 0;
    public int   Lives            = 3;
    public int   CurrentFloor     = 1;
    public float FloorTimeRemaining = 120f;

    public void LoadHiScore()
    {
        try { if (File.Exists(HiScoreFile)) HiScore = int.Parse(File.ReadAllText(HiScoreFile)); }
        catch { }
    }

    public void SaveHiScore()
    {
        try { File.WriteAllText(HiScoreFile, HiScore.ToString()); }
        catch { }
    }
}
