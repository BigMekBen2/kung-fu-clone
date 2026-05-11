namespace KungFuClone.Core;

public enum GameStateId { Title, Playing, BonusStage, GameOver }

public class GameContext
{
    public int   Score            = 0;
    public int   HiScore          = 0;
    public int   Lives            = 3;
    public int   CurrentFloor     = 1;
    public float FloorTimeRemaining = 120f;
}
