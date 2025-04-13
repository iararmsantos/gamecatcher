using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance {get; private set;}
	
	private PackedScene _mainScene = GD.Load<PackedScene>("res://Scene/Main/Main.tscn"); 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	public static void LoadMain() {
		Instance.GetTree().ChangeSceneToPacked(Instance._mainScene);
	}
}
