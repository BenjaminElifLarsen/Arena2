using Godot;
using System;

public class SecondBoss : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public int Score;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//NewGame();
		GD.Randomize();
	}
	// Called when the node enters the scene tree for the first time.
	private void GameWon()
	{
		var boss = GetNode<Boss2>("Boss2");
		GetNode<Timer>("ScoreTimer").Stop();
		boss.Hide();
		HighScore.CurrentScore += Score;
		GetTree().ChangeScene("res://Victory/Victory.tscn");
	}

	private void GameOver()
	{
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
	}

	public void NewGame()
	{
		GetTree().CallGroup("mobs", "queue_free");
		GetTree().CallGroup("friendly", "queue_free");
		Score = 0;
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(Score);
		hud.ShowMessage("Get Ready!");

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Position2D>("StartPosition");
		player.Start(startPosition.Position);
		
		var boss = GetNode<Boss2>("Boss2");
		var bossStartPosition = GetNode<Position2D>("bossStartPosition");
		boss.Start(bossStartPosition.Position);
		GetNode<Timer>("StartTimer").Start();
	}
	
	public void OnStartTimerTimeout()
	{
		GetNode<Timer>("ScoreTimer").Start();
	}
	
	public void OnScoreTimerTimeout()
	{
		Score++;
		GetNode<HUD>("HUD").UpdateScore(Score);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}


