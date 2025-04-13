using Godot;

public partial class Paddle : Area2D
{	
	[Export] float _speed = 100.0f;
	[Export] float _margin = 50.0f;

    //make paddle work as magnet for 10 seconds
    [Export] float _magnetRange;
    [Export] float _magnetForce = 300.0f; // Force of the magnet effect
    private bool _isMagnetActive = false;
    private float _magnetDuration = 10.0f; // Duration of the magnet effect in seconds
    private float _magnetTimer = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{        
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        //set magnet range to viewport size
        if (_magnetRange == 0.0f)
        {
            _magnetRange = GetViewportRect().Size.X;
        }
		// Move the paddle
        MovePaddle(delta);

        // Keep the paddle inside the screen
        KeepPaddleInScreen();

        // Update the magnet effect
        UpdateMagnetEffect(delta);        
	}

    private void KeepPaddleInScreen()
    {
        Rect2 viewportRect = GetViewportRect();
        // horizontally repositions the object so that its X-coordinate aligns with the left boundary of viewportRect, shifted by _margin
        //If _margin is positive, the object moves rightward.
        if (Position.X < viewportRect.Position.X + _margin)
        {
            Position = new Vector2(viewportRect.Position.X + _margin, Position.Y);
        }
        //If _margin is negative, the object moves leftward.   
        if (Position.X > viewportRect.End.X - _margin)
        {
            Position = new Vector2(viewportRect.End.X - _margin, Position.Y);
        }
    }

    /*
    Move paddle from left to right and from right to left
    */
    void MovePaddle(double delta)
    {
        if (Input.IsActionPressed("right"))
        {
            Position += new Vector2(_speed * (float)delta, 0);
        }

        if (Input.IsActionPressed("left"))
        {
            Position -= new Vector2(_speed * (float)delta, 0);
        }
    }

    /*
    Activate Magnet
    */
    public void ActivateMagnet()
    {
        if (_isMagnetActive)
        {
            return; // Magnet is already active
        }
        _isMagnetActive = true;
        _magnetTimer = _magnetDuration;
    }

    /*
    Update Magnet effect - will not activate again if it is already activated
    */
    private void UpdateMagnetEffect(double delta)
    {;
        if (_isMagnetActive)
        {
            _magnetTimer -= (float)delta;
            if (_magnetTimer <= 0)
            {
                _isMagnetActive = false;
            }
            else
            {
                ApplyMagnetEffect();
            }
        }
    }

    /*
    When user get a special gem it will activate a magnet effect (all gems go to the paddle).
    */
    private void ApplyMagnetEffect()
    {        
        foreach (Node node in GetTree().GetNodesInGroup("gems"))
        {            
            if (node is Gem gem)
            {                
                Vector2 direction = Position - gem.Position;
                float distance = direction.Length();                

                if (distance < _magnetRange)
                {
                    direction = direction.Normalized();
                    gem.Position += direction * _magnetForce * (float)GetProcessDeltaTime();
                }
            }
        }
    }
}
