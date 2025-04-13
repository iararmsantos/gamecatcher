using Godot;
using System;

public partial class Gem : Area2D
{
	[Export] float _speed = 110.0f;

	private Sprite2D sprite;

	public enum GemType
    {
        Red,
        Blue,
        Green,
		Yellow,
        Orange,
        Black,
		Super        
    }
    public GemType Type { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = new Sprite2D();
        AddChild(sprite);	

		// Add gem to the "gems" group
        AddToGroup("gems");	

		//connect signal
		AreaEntered += OnAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//The object moves downward (if _speed is positive) or upward (if _speed is negative)
		Position += new Vector2(0, _speed * (float)delta);
		CheckHitBottom();
	}

	//Signal created and connected in C#
    private void OnAreaEntered(Area2D area)
    {
		ScoreManager.IncrementScore();
		SignalManager.EmitOnGemCollected(this);		
		//destruct this instance - gem disappears when hit paddle
		QueueFree();
    }

	// Check if gem is out of screen
	public void CheckHitBottom()
	{
		if (Position.Y > GetViewportRect().Size.Y)
		{
			//emit OnGemOffScreenEventHandler signal
			LifeManager.DecrementLives();
			//destruct this instance - gem disappears when out of screen
			SetProcess(false);
			//destruct this instance - gem disappears when hit bottom			
			QueueFree();
		}
	}

	// to change sprites for the gem
	public void SetGemType(GemType type)
    {
        Type = type;
		string texturePath = type switch
		{
			GemType.Red => "res://assets/gems/element_red_diamond.png",
			GemType.Blue => "res://assets/gems/element_blue_diamond.png",
			GemType.Green => "res://assets/gems/element_green_diamond.png",
			GemType.Yellow => "res://assets/gems/element_red_diamond.png",
			GemType.Orange => "res://assets/gems/element_blue_diamond.png",
			GemType.Black => "res://assets/gems/element_green_diamond.png",
			GemType.Super => "res://assets/gems/element_blue_diamond.png",
			_ => throw new ArgumentOutOfRangeException(nameof(type), $"Unexpected GemType value: {type}")
		};

        sprite.Texture = GD.Load<Texture2D>(texturePath);
    }
}
