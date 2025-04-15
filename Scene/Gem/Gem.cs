using Godot;
using System;

public partial class Gem : Area2D
{
	[Export] float originalSpeed = 90.0f;
	private float fallSpeed = 90.0f;

	private Sprite2D sprite;

	public enum GemType
    {
        Red,
        Blue,
        Green,
		Yellow,
        Orange,
        Black,
		Super,
		Life ,
		Slow     
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
		Position += new Vector2(0, fallSpeed * (float)delta);
		CheckHitBottom();
	}

	/// <summary>
	/// Action to take when hit the paddle
	/// <param name="area"> Are which the gem touched.</param>
	/// </summary>
    private void OnAreaEntered(Area2D area)
    {
		ScoreManager.IncrementScore();
		SignalManager.EmitOnGemCollected(this);		
		//destruct this instance - gem disappears when hit paddle
		QueueFree();
    }

	/// <summary>
	/// Check ig gem hit the botton of the screed
	/// </summary>
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

	/// <summary>
	/// Reduce the gem speed
	/// <param name="type"> The gem type.</param>
	/// </summary>
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
			GemType.Life => "res://assets/gems/element_red_diamond.png",
			GemType.Slow => "res://assets/gems/element_green_diamond.png",
			_ => throw new ArgumentOutOfRangeException(nameof(type), $"Unexpected GemType value: {type}")
		};

        sprite.Texture = GD.Load<Texture2D>(texturePath);
    }

	/// <summary>
	/// Reduce the gem speed
	/// <param name="multiplier"> How much to slow down.</param>
	/// </summary>
	public void SlowDown(float multiplier)
	{
		fallSpeed = originalSpeed * multiplier;
		GD.Print($"Gem slowed down, new fall speed: {fallSpeed}");
	}

	/// <summary>
	/// Reset the gem's speed back to normal
	/// </summary>
    public void ResetSpeed()
    {
        fallSpeed = originalSpeed;
        GD.Print($"Gem speed reset to normal: {fallSpeed}");
    }

	/// <summary>
	/// Set the gem's speed
	/// <param name="speed">Speed goal</param>
	/// </summary>
	public void SetSpeed(float speed)
    {
        fallSpeed = speed;
        originalSpeed = speed;
    }

	/// <summary>
	/// Get the original gem speed
	/// </summary>
	/// <returns>The original gem speed.</returns>	
	public float GetSpeed() {
		return originalSpeed;
	}
}
