using Godot;

public partial class LevelButton : TextureButton
{
	static readonly Vector2 HOVER_SCALE = new Vector2(0.35f, 0.35f);
	static readonly Vector2 NORMAL_SCALE = new Vector2(0.25f, 0.25f);
	[Export] private int _levelNumber {get; set;}
	[Export] private Label _levelLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//handle infinite mode and other levels
		if (_levelNumber == 0) {			
			_levelLabel.Text = "Infinite";
		} else {
			_levelLabel.Text = _levelNumber.ToString();
		}		
		
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		Pressed += OnMousePressed;
	}

	//Open the selected level
    private void OnMousePressed()
    {		
        ScoreManager.SetLevelSelected(_levelNumber);
		GetTree().ChangeSceneToFile($"res://Scene/Level/Level{_levelNumber}.tscn");
    }

	//Decrease the button when hovering out
    private void OnMouseExited()
    {
        Scale = NORMAL_SCALE;
    }

	//Iecrease the button when hovering
    private void OnMouseEntered()
    {
        Scale = HOVER_SCALE;
    }
}