using System.Collections.Generic;

public class LevelGemGoal
{
    public int Level { get; set; }
    public Dictionary<Gem.GemType, int> GemAmounts { get; set; }

    public LevelGemGoal(int level)
    {
        Level = level;
        GemAmounts = new Dictionary<Gem.GemType, int>();
    }
}
