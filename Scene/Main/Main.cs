using Godot;

public partial class Main : Control
{
	[Export] private Label _highScoreLabel;
	[Export] private AudioStreamPlayer2D _backgroundSound;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		var img = ImageManager.GetRandomItemImage();
		var img2 = ImageManager.GetRandomItemImage();
		var img3 = ImageManager.GetImage(3);
		ImageManager.ShuffleImages();
		var img4 = ImageManager.GetImage(3);
		_highScoreLabel.Text = $"{ScoreManager.GetHighestScoreOfAllLevels()}";
	}

    public override void _ExitTree()
    {
        _backgroundSound.Stop();
    }

}
