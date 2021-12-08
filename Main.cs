using Godot;
using System;

public class Main : Node
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

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void GameWon()
	{
		GetNode<Timer>("ScoreTimer").Stop();
		HighScore.CurrentScore += Score;
		GetTree().ChangeScene("res://SecondScreen/SecondBoss.tscn");
		var boss = GetNode<Boss1>("Boss1");
		boss.Stop();
	}

	private void GameOver()
	{
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
		var boss = GetNode<Boss1>("Boss1");
		boss.Stop();
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
		
		var boss = GetNode<Boss1>("Boss1");
		var bossStartPosition = GetNode<Position2D>("BossStartPosition");
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
	


}


