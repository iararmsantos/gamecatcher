
using System;

public class LevelScore
{
    public int LevelNumber {get; set;}
    public int HighScore {get; set;}
    public DateTime DateSet {get; set;}

    public LevelScore(int levelNumber, int highScore)
    {
        DateSet = DateTime.Now;
        LevelNumber = levelNumber;
        HighScore = highScore;
    }
}
