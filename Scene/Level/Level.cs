using System.Collections.Generic;
using Godot;

public partial class Level : Node2D
{
	//static and readonly: to open imediately when the first instance of the class is created then it will never be loaded again
	private static readonly AudioStream EXPLODE_SOUND = GD.Load<AudioStream>("res://assets/sound/explode.wav");
	const double VIEWPORT_MARGIN = 50.0f;	
    //to spawn new gems
	[Export] private PackedScene _gemScene;
	//timer referencing the amount of time a new gem will show in the screen
	[Export] private Timer _spawnTimer;
	[Export]private AudioStreamPlayer _music;
	[Export]private AudioStreamPlayer2D _effect;
	[Export] private Node2D _gemsHolder;

	private float _timeToGenerateGemLevel = 5;
	private Gem _lastCollectedGem;	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {   
        SpawnTimerHandler();
        SpawnGem();

        ScoreManager.ResetScore();
        LifeManager.ResetLives();
        SignalManager.Instance.OnEndGame += OnEndGame;
        SignalManager.Instance.OnScore += OnScore;
		SignalManager.Instance.OnGameOver += OnGameOver;
    }

	public override void _ExitTree()
	{
		SignalManager.Instance.OnEndGame -= OnEndGame;
		SignalManager.Instance.OnGemCollected -= OnGemCollected;
        SignalManager.Instance.OnScore -= OnScore;
		SignalManager.Instance.OnGameOver -= OnGameOver;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Escape)) {
			if (ScoreManager.GetLevelSelected() == 0) {
				SignalManager.EmitOnEndGame();				
			} else {
				GameManager.LoadMain();			
			}			
		}			
	}

    private void OnGameOver()
    {
        _effect.Stop();

		_effect.Stream = EXPLODE_SOUND;
		_effect.Play();		
    }

    private void SpawnTimerHandler()
    {
        // Set a random wait time between 1 and 5 seconds
        if (_spawnTimer == null)
        {
            _spawnTimer = new Timer();
            AddChild(_spawnTimer);
        }
        _spawnTimer.WaitTime = (float)GD.RandRange(1.0, 2.0);
        _spawnTimer.Timeout += SpawnGem;
    }

	/**
	 * Spawn gems in the screen in random positions, time, and types
	 */
	private void SpawnGem()
	{
		//create new instance of gem
		Gem gem = (Gem)_gemScene.Instantiate();
		Rect2 viewportRect = GetViewportRect();
		gem.SetSpeed(gem.GetSpeed());
		
		//add gem to scene
		_gemsHolder.AddChild(gem);	

		//randomize position of gem
		float randomViewportRange = (float)GD.RandRange(viewportRect.Position.X + VIEWPORT_MARGIN, viewportRect.End.X - VIEWPORT_MARGIN);	
		
		//change position of gem
		gem.Position = new Vector2(randomViewportRange, -100);

		// generate random gem types
		Gem.GemType randomType = (Gem.GemType)GD.RandRange(0, System.Enum.GetValues(typeof(Gem.GemType)).Length - 1);
	
		gem.SetGemType(randomType);

		SignalManager.Instance.OnGemCollected += OnGemCollected;
		
		// Set a new random wait time between 1 and 2 seconds
    	_spawnTimer.WaitTime = (float)GD.RandRange(1.0, _timeToGenerateGemLevel);	
    	_spawnTimer.Start(); // Restart the timer			
	}

    /**
	 * Get onScore signal, play sound effect, increase level
	 */
    private void OnScore()
    {
		ScoreManager.IncrementIncreaseCount();
        // Check if _effect is not null and is still valid before calling Play
        if (_effect != null && IsInstanceValid(_effect))
        {
            _effect.Play();
        }
		
        // increase level
        if (ScoreManager.GetIncreaseCount() > 0 && ScoreManager.GetIncreaseCount() % 10 == 0 && _timeToGenerateGemLevel > 0)
        {			
            --_timeToGenerateGemLevel;
			GD.Print($"Level {_timeToGenerateGemLevel}");
        }

        HandleBonusGemType();
    }

	/**
	 * Handle Different Bonus by Gem Types
	 */
    private void HandleBonusGemType()
    {        
        Gem lastCollectedGem = GetLastCollectedGem();
    	 if (lastCollectedGem != null)
        {
            ValidateGemMagnet(lastCollectedGem);
            ValidateGemLife(lastCollectedGem);
            ValidateGemSlow(lastCollectedGem);
        }
    }

    private void ValidateGemSlow(Gem lastCollectedGem)
    {        
        if (lastCollectedGem.Type == Gem.GemType.Slow)
        {
            // Check if the Game node is still valid before accessing the Paddle node
            if (IsInstanceValid(this))
            {
                // Make gems fall slowly
                // Get all active gems in the scene
                var gemNodes = GetTree().GetNodesInGroup("gems");
				List<Gem> slowedGems = new();

                foreach (Node gemNode in gemNodes)
                {
                    // Check if the node is of type Gem
                    if (gemNode is Gem gem)
                    {
                        // Apply slowdown effect to all gems
                        GD.Print($"Slowing down gem: {gem}");
                        gem.SlowDown(0.25f); // Apply the slowdown effect (you can adjust the factor as needed)
						slowedGems.Add(gem);
                    } else {
							GD.Print("Could not cast gem to slow down.");
						}
                }

                // Reset speed after 5 seconds
                var timer = new Timer();
                AddChild(timer);
                timer.WaitTime = 5.0f;
                timer.OneShot = true;
                timer.Timeout += () =>
                {
                    foreach (Gem gem in slowedGems)
                    {
                        if (IsInstanceValid(gem))
						{
							gem.ResetSpeed();
						}
                    }
                    timer.QueueFree();
                };
                timer.Start();
            }
        }
    }


    private void ValidateGemLife(Gem lastCollectedGem)
    {
        // extra life
        if (lastCollectedGem.Type == Gem.GemType.Life)
        {
            // make all bonus action happen
            // GD.Print("Gem is Life");

            // Check if the Game node is still valid before accessing the Paddle node
            if (IsInstanceValid(this))
            {
                LifeManager.IncrementLives();
            }
        }
    }


    private void ValidateGemMagnet(Gem lastCollectedGem)
    {
        // if (lastCollectedGem.Type == Gem.GemType.Super)
        // {
        //     // make all bonus action happen
        //     GD.Print("Gem is Super");

        //     // Check if the Game node is still valid before accessing the Paddle node
        //     if (IsInstanceValid(this))
        //     {
        //         // Get the paddle node and activate the magnet
        //         Paddle paddle = GetNode<Paddle>("Paddle");
        //         if (paddle != null)
        //         {
        //             paddle.ActivateMagnet();
        //         }
        //     }
        // }
    }

    /**
	 * Stop actions on GameOver
	 */

    private void OnEndGame()
	{
		// stop all nodes and timer	
		foreach (Node node in GetChildren())
		{
			node.SetProcess(false);						
		}
		
		StopGems();
		
		_spawnTimer.Stop();
		_music.Stop();
	}

	/**
	 * Stop all falling gems
	 */
    private void StopGems()
    {
		// this will be the same process to make all gems disappear when get some bonus
		// I will try to make the paddle work as magnet 
        foreach(Gem gem in _gemsHolder.GetChildren()) {
			gem.SetProcess(false);
		}
    }

	private void OnGemCollected(Gem gem)
    {
        _lastCollectedGem = gem;
    }

    private Gem GetLastCollectedGem()
    {
        return _lastCollectedGem;
    }
}
