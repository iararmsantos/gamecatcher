using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

//
public partial class Scorer : Node
{
    private static LevelGemGoal _generatedGoal;
	private static Random rng = new Random();
    private Gem _lastCollectedGem;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        SetGoal();        
        SignalManager.Instance.OnGemCollected += OnGemCollected;
    }

    public override void _ExitTree()
    {
        SignalManager.Instance.OnGemCollected -= OnGemCollected;
    }

    /*
    Will check every collected gem
    */
    private void OnGemCollected(Gem gem)
    {
        _lastCollectedGem = gem;
        HandleGemCollected();        
        CheckGoalCompletion();               
    }

    /*
    Emit signal every time a caught gem is in the goal to update the label in GameUi
    */
    private void HandleGemCollected() {
        var type = _lastCollectedGem?.Type;

        if (type == null) return;

        if (!ScoreManager.GoalContainsGemType(type.Value)) return;
        
        ScoreManager.RecordGemCaught(type.Value);
		GD.Print($"Gem Collected Type: {type}, Total Collected: {ScoreManager.GetRemainingGoal()[type.Value]}");
        
		SignalManager.EmitOnGoalUpdated(); // Can still emit to update UI
    }

    /*
    Verify if the gem complete the goal to emit game complete to finish the level
    */
    private void CheckGoalCompletion()
    {        
        var remaining = ScoreManager.GetRemainingGoal();
		bool allGoalsMet = remaining.Values.All(v => v == 0);

		if (allGoalsMet)
		{
			GD.Print("🎉 Level Complete!");            
            SignalManager.EmitOnLevelComplete();
			SignalManager.EmitOnEndGame();
            
            ScoreManager.SetScoreForLevel(ScoreManager.GetLevelSelected(), ScoreManager.GetScore());
		}
    }

    /*
    Get the goal to be used in any place
    */
    public static LevelGemGoal GetGoal() => _generatedGoal;

    /*
    Set the actual goal
    */
    public void SetGoal() {         
        int level = ScoreManager.GetLevelSelected();
		_generatedGoal = GenerateRandomGemGoal(level);
		ScoreManager.SetGemsGoal(_generatedGoal.GemAmounts);

		foreach (var (type, amount) in _generatedGoal.GemAmounts)
			GD.Print($"Gem: {type}, Amount: {amount}"); 

        SignalManager.EmitOnGoalUpdated();  
    }

    /*
    Generate random types and amount to compose the goal
    */
    public static LevelGemGoal GenerateRandomGemGoal(int level)
    {
        int maxTypes = level switch
        {
            1 => 2,
            2 => 3,
            3 => 4,
            4 => 4,
            5 => 5,
            _ => 1
        };

        int maxGems = level switch
        {
            1 => 5,
            2 => 20,
            3 => 30,
            4 => 40,
            5 => 50,
            _ => 10
        };
        
        //TODO: create a variable for the random minimum depending on the level because sometimes we are getting in the level 3 1 gem.
        int numTypes = rng.Next(1, maxTypes + 1);
        //get all types and cast it to Gem.GemType (because GetValues returns a generic Array) and convert it to a list
        var allGemTypes = Enum.GetValues(typeof(Gem.GemType)).Cast<Gem.GemType>().ToList();
        //random selection of numTypes gem types, with no duplicates
        var selectedGemTypes = allGemTypes.OrderBy(_ => rng.Next()).Take(numTypes).ToList();

        var gemAmounts = new Dictionary<Gem.GemType, int>();

        //generate amount from minGems to maxGems - uncomment this after tests
        // int minGems = maxGems / 3; //TODO: make this 3 a constant
        int minGems = maxGems;
        int totalGems = rng.Next(minGems, maxGems + 1);
        int remainingGems = totalGems;

        foreach (var gem in selectedGemTypes)
        {            
            // maxForThisType ensures we reserve at least 1 gem for each remaining type
            int maxForThisType = remainingGems - (selectedGemTypes.Count - gemAmounts.Count - 1);
            //ensures the current gem type gets At least 1 gem at most maxForThisType gem
            int amount = rng.Next(1, maxForThisType + 1);
            gemAmounts[gem] = amount;
            remainingGems -= amount;
        }

        return new LevelGemGoal(level) { GemAmounts = gemAmounts };
    }

    public Dictionary<Gem.GemType, int> GetRemainingGoal()
    {
        return ScoreManager.GetRemainingGoal();
    }
}
