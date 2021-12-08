using Godot;
using System;

public class Boss2Turret : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Vector2 EnemyLocation; 
	[Export]
	public PackedScene MobScene;
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
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		//float angle = (float)Math.Atan(xDistance/yDistance);
		
		var node = (AnimatedSprite)GetChild(0);
		var node2 = (Boss2)GetParent();
		Vector2 dir = EnemyLocation - node2.Position ;
		float xDistance = Math.Abs(EnemyLocation.x - node2.Position.x);
		float yDistance = Math.Abs(EnemyLocation.y - node2.Position.y);
		//node.Rotation = (float)Math.Atan2(dir.y,dir.x);
		//GD.Print("X: " + node2.Position.x + " Y: " + node2.Position.y);
		node.Rotation = (float)(dir.Angle() - (float)Math.PI*0.5);
		//GD.Print(node.Rotation);
		if(CurrentFiringCooldown <= 0)
		{
			var mobLeft = (Bullet)MobScene.Instance();
			GetParent().GetParent().AddChild(mobLeft);
			var xyLeft = Vector2.Zero;
			xyLeft.x = node2.Position.x;
			xyLeft.y = node2.Position.y;
			
			//GD.Print(node.Rotation);
			mobLeft.Position = xyLeft;
			mobLeft.Rotation = node.Rotation;
			float direction = mobLeft.Rotation;
			var totalSpeed = 400;
			var totalLength = (float)Math.Sqrt(Math.Pow(xDistance,2) + Math.Pow(yDistance,2));
			var xPercentage = Math.Abs(EnemyLocation.x - node2.Position.x) / totalLength;
			//GD.Print(xPercentage);
			if(xPercentage == 0)
			{
				mobLeft.XSpeed = 0;
			}
			else if (EnemyLocation.x - node2.Position.x < 0){
				mobLeft.XSpeed = totalSpeed * -xPercentage;
			}
			else if (EnemyLocation.x - node2.Position.x > 0){
				mobLeft.XSpeed = totalSpeed * xPercentage;
			}
			mobLeft.YSpeed = totalSpeed;
			CurrentBulletsInARow++;
			GetNode<AudioStreamPlayer>("../../Lazer").Play();
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
		if(CurrentFiringCooldown > 0)
		{
			CurrentFiringCooldown -= delta;
		}
	}
	public void SetLocation(Vector2 location)
	{
		EnemyLocation = location;
	}
}
