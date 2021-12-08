using Godot;
using System;

public class Boss2 : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Signal]
	public delegate void Killed();
	[Signal]
	public delegate void Taunt(string taunt);
	[Signal]
	public delegate void TakeDamage(int health);

	[Export]
	public int Speed = 250;
	public int Health;
	[Export]
	public int MaxHealth = 20;
	public sbyte Direction;
	public Vector2 ScreenSize; //size of the game window
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
		Direction = -1;
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var velocity = Vector2.Zero;
		if(Position.x == 0)
		{
			Direction = 1;
		}
		else if(ScreenSize.x == Position.x)
		{
			Direction = -1;
		}
		
		if(Direction > 0)
		{
			velocity.x += 1;
		}
		else
		{
			velocity.x -= 1;
		}
		
		var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite.Play();
		}
		else
		{
			animatedSprite.Stop();
		}
		Position += velocity * delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, 0, ScreenSize.x),
			y: Mathf.Clamp(Position.y, 0, ScreenSize.y)
		);
	}

	private void GetLocation(Vector2 position)
	{
		var node = (Boss2Turret)GetChild(2);
		node.SetLocation(position);
		// Replace with function body.
	}
	
	public void Start(Vector2 pos)
	{
		Position = pos;
		Health = MaxHealth;
		EmitSignal(nameof(TakeDamage),Health);
		Show();
		//CurrentBulletsInARow = 0;
		//CurrentFiringCooldown = 0;
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	
	private void OnBodyEntered(object body)
	{
		Health -= 1;
		GetNode<AudioStreamPlayer>("../Damage").Play();
		if(Health == 0)
		{
			
			EmitSignal(nameof(Killed));
			
		}
		else{
			
			EmitSignal(nameof(Taunt), "Well, well, well");
			EmitSignal(nameof(TakeDamage),Health);
		}
	}
}







