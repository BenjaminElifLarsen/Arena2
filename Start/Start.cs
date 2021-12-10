using Godot;
using System;

public class Start : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		GetNode<LineEdit>("LineEdit").Text = HighScore.Name;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void OnStartButtonPressed()
	{
		string name = GetNode<LineEdit>("LineEdit").Text;
		if(name.Trim() != "")
		{
			HighScore.Name = name;
		}
		GetTree().ChangeScene("res://Main.tscn");
		// Replace with function body.
	}
}


