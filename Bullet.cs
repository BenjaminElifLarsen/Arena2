using Godot;
using System;

public class Bullet : RigidBody2D
{
// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	[Export]
	public float YSpeed = 200;
	[Export]
	public float XSpeed = 0;
	[Export]
	public int Speed = 500;
	public Vector2 ScreenSize; //size of the game window
	
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		var animSprite2D = GetNode<AnimatedSprite>("AnimatedSprite");
		animSprite2D.Playing = true;
		string[] mobTypes = animSprite2D.Frames.GetAnimationNames();
		animSprite2D.Animation = mobTypes[GD.Randi() % mobTypes.Length];
	}
	
	public void OnVisibilityNotifier2DScreenExited()
	{
		QueueFree();
	}
	
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var velocity = Vector2.Zero;
		
		velocity.x += 1;
		velocity.y += 1;
		var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		velocity.x *= XSpeed;
		velocity.y *= YSpeed;
		velocity = velocity.Normalized() * Speed;
		animatedSprite.Play();
		Position += velocity * delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, 0, ScreenSize.x),
			y: Mathf.Clamp(Position.y, 0, ScreenSize.y)
		);
		if(Position.y <= 0 || Position.y >= ScreenSize.y)
		{
			QueueFree();
		}
		if(Position.x == 0 || Position.x == ScreenSize.x)
		{
			XSpeed *= -1;
			//var node = (AnimatedSprite)GetChild(2);
			//if(XSpeed < 0)
			//{
			//	node.FlipH = true;
			//}
			//else if(XSpeed > 0)
			//{
			//	node.FlipH = false;
			//}
		}
	}
}
