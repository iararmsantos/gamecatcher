using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ScoreManager : Node
{
	public static ScoreManager Instance { get; private set; }
	private const int SCORE_VALUE = 10;		
	// Public static property to get the SCORE_VALUE
    public static int ScoreValue => SCORE_VALUE;

	//save score to file
	private const int DEFAULT_SCORE = 0;
	private const string SCORE_FILE = "user://gemcatcherornot.save";
	static List<LevelScore> _levelScores = new List<LevelScore>();

	private int _score = 0;
	private int _highScore = 0;
	private int _increaseCount = 0;	
	private int _levelSelected;

	//handling gems goal
	private static Dictionary<Gem.GemType, int> _gemsGoal = new();
	private static Dictionary<Gem.GemType, int> _gemsCaught = new();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		LoadScoreFromFile();
	}

	public override void _ExitTree()
    {
        SaveScoreToFile();
    }

	//validate if the caught gem type is in gems Goal
	public static bool GoalContainsGemType(Gem.GemType type) { 
		GD.Print($"collected gem: {type}");
		return _gemsGoal.ContainsKey(type);
	}

	//Set the generated goal to _gemsGoal
	public static void SetGemsGoal(Dictionary<Gem.GemType, int> goal)
	{
		_gemsGoal = new Dictionary<Gem.GemType, int>(goal);
		_gemsCaught.Clear();
	}

	//save the gem caught
	public static void RecordGemCaught(Gem.GemType type)
	{
		if (_gemsCaught.ContainsKey(type))
			_gemsCaught[type]++;
		else
			_gemsCaught[type] = 1;
	}

	//Get the remaining goal 
	public static Dictionary<Gem.GemType, int> GetRemainingGoal()
	{
		var remaining = new Dictionary<Gem.GemType, int>();
		foreach (var kvp in _gemsGoal)
		{
			var type = kvp.Key;
			var required = kvp.Value;
			var collected = _gemsCaught.ContainsKey(type) ? _gemsCaught[type] : 0;
			remaining[type] = Math.Max(required - collected, 0);
		}
		return remaining;
	}	

	/* Score System */
	public static int GetIncreaseCount()
	{
		return Instance._increaseCount;
	}

	public static void SetIncreaseCount(int value)
	{
		Instance._increaseCount = value;
	}
	
	public static void IncrementIncreaseCount()
	{
		SetIncreaseCount(GetIncreaseCount() + 1);
	}

	public static int GetScore()
	{
		return Instance._score;
	}

	public static int GetHighScore()
	{
		return Instance._highScore;
	}

	/*
	Find first score by level number or return null
	*/
	public static LevelScore GetLevelScore(int levelNumber) {
		return _levelScores.FirstOrDefault(ls => ls.LevelNumber == levelNumber);
	}

	/*
	Find first score by level number or return null
	*/
	public static int GetHighestScoreOfAllLevels()
	{
		if (_levelScores == null || !_levelScores.Any())
        return 0;

    	return _levelScores.Max(ls => ls.HighScore);
	}

	public static int GetLevelHighScore(int level)
	{
		LevelScore levelScore = GetLevelScore(level); 

		if (levelScore != null) {
			return levelScore.HighScore;
		}

		return DEFAULT_SCORE;
	}

	public static void SetScoreForLevel(int levelNumber, int score)
	{
		LevelScore levelScore = GetLevelScore(levelNumber);

		//update if exist
		if (levelScore != null) {			
			if (score > levelScore.HighScore) {
				levelScore.HighScore = score;
				levelScore.DateSet = DateTime.Now;				
			}
		//create new if doesn't exist
		} else {
			_levelScores.Add(new LevelScore(levelNumber, score));
		}
	}

	public static void SetScore(int value)
	{
		Instance._score = value;

		if (Instance._score > Instance._highScore)
		{
			Instance._highScore = Instance._score;
		}
		
		SignalManager.EmitOnScore();
	}

	public static void ResetScore()
	{
		SetScore(0);
	}

	public static void IncrementScore()
	{		
		SetScore(GetScore() + SCORE_VALUE);
	}	

	private void SaveScoreToFile() {
		using FileAccess file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Write);

		if (file != null) {			
			string jsonStr = JsonConvert.SerializeObject(_levelScores);
			file.StoreString(jsonStr);
		} else {
			GD.Print("failed to save file");
		}
	}

	private void LoadScoreFromFile() {
	using FileAccess file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Read);

	if (file != null) {
		string jsonStr = file.GetAsText();

		if (!string.IsNullOrEmpty(jsonStr)) {
			_levelScores = JsonConvert.DeserializeObject<List<LevelScore>>(jsonStr);
		}
	} else {
		GD.Print("failed to open file");
	}
}

	//Phases system
	public static int GetLevelSelected() {
		return Instance._levelSelected;
	}

	public static int SetLevelSelected(int level) {
		return Instance._levelSelected = level;
	}
}
