using Godot;
using System;

public class Victory : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var scores = new HighScore();
		scores.LoudScores();
		scores.AddScore(HighScore.Name,HighScore.CurrentScore);
		scores.SaveScores();
		GetNode<Label>("ScoreLabel").Text = HighScore.CurrentScore.ToString();
		string text = "";
		foreach(var score in scores.Scores){
			text += score.Name + " : " + score.Value + "\n";
		}
		GetNode<Label>("ScoresLabel").Text = text;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void OnButtonPressed()
	{
		GetTree().Quit();
	}
}


