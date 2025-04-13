using Godot;

public partial class SignalManager : Node
{
	public static SignalManager Instance { get; private set; }

	//custom signal to check if gem is out of screen
	[Signal] public delegate void OnGemOffScreenEventHandler();
	[Signal] public delegate void OnScoreEventHandler();
	[Signal] public delegate void OnEndGameEventHandler();
	[Signal] public delegate void OnLevelCompleteEventHandler();
	[Signal] public delegate void OnGemCollectedEventHandler(Gem gem);
	[Signal] public delegate void OnGoalUpdatedEventHandler();
	[Signal] public delegate void OnGameOverEventHandler();
	[Signal] public delegate void OnLivesUpdatedEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	public static void EmitOnGemOffScreen()
	{
		Instance.EmitSignal(SignalName.OnGemOffScreen);
	}

	public static void EmitOnScore()
	{
		Instance.EmitSignal(SignalName.OnScore);
	}

	public static void EmitOnEndGame()
	{
		Instance.EmitSignal(SignalName.OnEndGame);
	}

	public static void EmitOnLevelComplete()
	{
		Instance.EmitSignal(SignalName.OnLevelComplete);
	}

	public static void EmitOnGemCollected(Gem gem)
	{
		Instance.EmitSignal(SignalName.OnGemCollected, gem);
	}

	public static void EmitOnGoalUpdated()
	{
		Instance.EmitSignal(SignalName.OnGoalUpdated);
	}

	public static void EmitOnGameOver()
	{
		Instance.EmitSignal(SignalName.OnGameOver);
	}

	public static void EmitOnLivesUpdated()
	{
		Instance.EmitSignal(SignalName.OnLivesUpdated);
	}
}
