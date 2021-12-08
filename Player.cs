using Godot;
using System;

public class Player : Area2D
{
	
	
	[Signal]
	public delegate void Hit();
	[Signal]
	public delegate void TakeDamage(int health);
	[Signal]
	public delegate void Location(Vector2 position);
	
	[Export]
	public PackedScene MobScene;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public int MaxHeatlh = 10;
	[Export]
	public int Speed = 400; //p/s
	public int Health = 10;
	[Export]
	public float InvisableLength = 2; //seconds
	public float CurrentInvisable = 0;
	[Export]
	public float FiringCooldown = 0.24f;
	public float CurrentFiringCooldown = 0;
	
	public Vector2 ScreenSize; //size of the game window
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		Health = MaxHeatlh;
		ScreenSize = GetViewportRect().Size;
		EmitSignal(nameof(TakeDamage),Health);
		//GetNode<HUD>("HUD").UpdatePlayerHeatlh(Health);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var velocity = Vector2.Zero;
		if(Input.IsActionPressed("ui_right"))
		{
			velocity.x += 1;
		}
		
		if(Input.IsActionPressed("ui_left"))
		{
			velocity.x -= 1;
		}
		if(Input.IsActionPressed("ui_up") && CurrentFiringCooldown <= 0)
		{
			var mobLeft = (FriendlyBullet)MobScene.Instance();
			var mobRight = (FriendlyBullet)MobScene.Instance();
			GetParent().AddChild(mobLeft);
			GetParent().AddChild(mobRight);
			var xyLeft = Vector2.Zero;
			xyLeft.x = Position.x-30;
			xyLeft.y = Position.y-60;
			var xyRight = Vector2.Zero;
			xyRight.x = Position.x+30;
			xyRight.y = Position.y-60;
			mobLeft.Position = xyLeft;
			mobRight.Position = xyRight;
			CurrentFiringCooldown = FiringCooldown;
			GetNode<AudioStreamPlayer>("../LazerPlayer").Play();
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
		if(CurrentInvisable > 0)
		{
			CurrentInvisable -= delta;
		}
		if(CurrentFiringCooldown > 0)
		{
			CurrentFiringCooldown -= delta;
		}
		EmitSignal(nameof(Location),Position);
	}
	
	
	public void Start(Vector2 pos)
	{
		Position = pos;
		Health = MaxHeatlh;
		EmitSignal(nameof(TakeDamage),Health);
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void OnPlayerBodyEntered(object body)
	{
		if(CurrentInvisable <= 0)
		{
			Health -= 1;
			CurrentInvisable = InvisableLength;
			EmitSignal(nameof(TakeDamage),Health);
			GetNode<AudioStreamPlayer>("../Damage").Play();
			//GetNode<HUD>("HUD").UpdatePlayerHeatlh(Health);
		// Replace with function body.
		}
		if(Health == 0)
		{
			EmitSignal(nameof(Hit));
			Hide();
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
		}
	}
		
}

