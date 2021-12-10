using Godot;
using System;

public class MoveSprit : Sprite
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public int Speed = 50;
	public Vector2 ScreenSize; //size of the game window
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var velocity = Vector2.Zero;
		velocity.y -= 1;
		velocity.x += 0;
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
		}
		Position += velocity * delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, -300, ScreenSize.x+1500),
			y: Mathf.Clamp(Position.y, -500, ScreenSize.y+1500)
		);
	}
}
