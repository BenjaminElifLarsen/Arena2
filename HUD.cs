using Godot;
using System;

public class HUD : CanvasLayer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Signal]
	public delegate void StartGame();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();

		GetNode<Timer>("MessageTimer").Start();
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
	public void UpdateBossHealth(int hp)
	{
		GetNode<Label>("BossHealth").Text = hp.ToString();
	}
	
	public void UpdatePlayerHeatlh(int hp)
	{
		GetNode<Label>("PlayerHealth").Text = hp.ToString();
	}
	
	async public void ShowGameOver()
	{
		ShowMessage("Game Over");

		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, "timeout");

		var message = GetNode<Label>("Message");
		message.Text = "Fight, Win, Survive";
		message.Show();

		await ToSignal(GetTree().CreateTimer(1), "timeout");
		GetNode<Button>("StartButton").Show();
	}
	
	private void OnMessageTimerTimeout()
	{
		// Replace with function body.
		GetNode<Label>("Message").Hide();
	}


	private void OnStartButtonPressed()
	{
		// Replace with function body.
		GetNode<Button>("StartButton").Hide();
		EmitSignal("StartGame");
	}

	
	
	
}

