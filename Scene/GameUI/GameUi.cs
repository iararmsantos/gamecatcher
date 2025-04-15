using Godot;
using System;
using System.Collections.Generic;

public partial class GameUi : Control
{
	[Export]private Label _lifeLabel;
	[Export]private Label _goalLabel;
	[Export]private Label _scoreLabel;
	[Export]private VBoxContainer _gameOverVBox;
	[Export]private Label _levelCompleteLabel;
	[Export]private Label _gameOverLabel;

	//TODO: play a sad music when gameOver and a happy music when GameComplete
	[Export]private AudioStreamPlayer _gameOverSound;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_lifeLabel.Text = $"{LifeManager.GetLives():00}";

		SignalManager.Instance.OnLevelComplete += OnLevelComplete;
		SignalManager.Instance.OnGameOver += OnGameOver;
		SignalManager.Instance.OnLivesUpdated += OnLivesUpdated;
		SignalManager.Instance.OnGoalUpdated += OnGoalUpdated;
		SignalManager.Instance.OnScore += OnScore;
	}

	public override void _ExitTree()
    {
        SignalManager.Instance.OnLevelComplete -= OnLevelComplete;
		SignalManager.Instance.OnGameOver -= OnGameOver;
		SignalManager.Instance.OnLivesUpdated -= OnLivesUpdated;
		SignalManager.Instance.OnGoalUpdated -= OnGoalUpdated;
		SignalManager.Instance.OnScore -= OnScore;
    }

    private void OnScore()
    {
        _scoreLabel.Text = $"{ScoreManager.GetScore():0000}";
    }

    //Update the label every time a gem caught is in the goal type

    private void OnGoalUpdated()
    {
        // Dictionary<Gem.GemType, int> gemsGoal = Scorer.GetGoal().GemAmounts;
		Dictionary<Gem.GemType, int> gemsGoal = ScoreManager.GetRemainingGoal();

		string goalText = "";
        foreach (var kvp in gemsGoal)
        {     			   
			goalText += $"{kvp.Key.ToString()[0]}: {kvp.Value}  ";
        }

		// GD.Print(goalText);
		_goalLabel.Text = goalText;
    }


    private void OnLivesUpdated()
    {
        _lifeLabel.Text = $"{LifeManager.GetLives():00}";
    }

    private void OnGameOver()
    {
        _levelCompleteLabel.Hide();
        _gameOverVBox.Show();
		_gameOverSound.Play();
    }


    private void OnLevelComplete()
    {		
		_gameOverLabel.Hide();
        _gameOverVBox.Show();
		_gameOverSound.Play();
    }

	public override void _Process(double delta)
	{
		if (_gameOverVBox.Visible && Input.IsKeyPressed(Key.Space)) {
			GameManager.LoadMain();
		}
	}
}
