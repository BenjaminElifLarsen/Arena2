using Godot;
using System;

public class Boss1 : Area2D
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
	public Vector2 ScreenSize; //size of the game window
	[Export]
	public PackedScene MobScene;
	[Export]
	public int Speed = 400;
	public int Health;
	[Export]
	public int MaxHealth = 20;
	public sbyte Direction;
	[Export]
	public float FiringCooldown = 0.027f;
	public float CurrentFiringCooldown = 0; 
	[Export]
	public int MaxBulletsInARow = 50;
	public int CurrentBulletsInARow = 0;
	[Export]
	public float ReloadCooldown = 0.83f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Direction = 1;
		Hide();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if(CurrentFiringCooldown <= 0)
		{
			var mobLeft = (Bullet)MobScene.Instance();
			GetParent().AddChild(mobLeft);
			var xyLeft = Vector2.Zero;
			int offset = CurrentBulletsInARow % 3 ;
			if(offset == 0) //right
			{
				xyLeft.x = Position.x-48;
				xyLeft.y = Position.y-6;
				mobLeft.XSpeed = -20;
			}
			else if (offset == 2) //left
			{
				xyLeft.x = Position.x+46;
				xyLeft.y = Position.y-10;
				mobLeft.XSpeed = 20;
			}
			else if (offset == 1) //center
			{
				xyLeft.x = Position.x;
				xyLeft.y = Position.y+14;
				mobLeft.XSpeed = 0;
			}
			mobLeft.Position = xyLeft;
			CurrentBulletsInARow++;
			GetNode<AudioStreamPlayer>("../Lazer").Play();
			if(CurrentBulletsInARow == MaxBulletsInARow)
			{
				CurrentFiringCooldown = ReloadCooldown;
				CurrentBulletsInARow = 0;
			}
			else
			{
				CurrentFiringCooldown = FiringCooldown;
			}
		}
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
		if(CurrentFiringCooldown > 0)
		{
			CurrentFiringCooldown -= delta;
		}
	}

	public void Start(Vector2 pos)
	{
		Position = pos;
		Health = MaxHealth;
		EmitSignal(nameof(TakeDamage),Health);
		Show();
		CurrentBulletsInARow = 0;
		CurrentFiringCooldown = 0;
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	
	private void OnBoos1BodyEntered(object body)
	{
		// Replace with function body.
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


