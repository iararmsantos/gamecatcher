using Godot;

public partial class LifeManager : Node
{
	public static LifeManager Instance { get; private set; }
	private int _lives = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	public static int GetLives()
	{
		return Instance._lives;
	}

	public static void SetLives(int value)
	{
		Instance._lives = value;	
		if (GetLives() <= 0)
    	{
			SignalManager.EmitOnGameOver();
       		SignalManager.EmitOnEndGame();
    	}
		SignalManager.EmitOnLivesUpdated();
	}

	public static void ResetLives()
	{
		SetLives(3);
	}

	public static void DecrementLives()
	{
		SetLives(GetLives() - 1);
		SignalManager.EmitOnGemOffScreen();
	}

	public static void IncrementLives()
	{
		if (GetLives() < 3) {
			SetLives(GetLives() + 1);
		}		
	}
}
