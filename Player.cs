using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed {get; set;} = 400; //how fast the player will move in pixels/sex
	public Vector2 ScreenSize; //size of the game window
	[Signal]
	public delegate void HitEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;

		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; //The player's movement vector

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		var AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			AnimatedSprite2D.Play();
		}
		else
		{
			AnimatedSprite2D.Stop();
		}

		Position += velocity * (float)delta;
		Position = new Vector2 (
			x: Mathf.Clamp(Position.X, 0,ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0 , ScreenSize.Y)
		);

        AnimatedSprite2D.FlipV = velocity.Y > 0;
		if (velocity.X != 0)
		{
			AnimatedSprite2D.Animation = "walk";
			AnimatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			AnimatedSprite2D.Animation = "up";
		}
	}
	
	private void OnBodyEntered(Node2D body)
	{
		Hide(); //player disappears after being hit
		EmitSignal(SignalName.Hit);
		// Can't change physics on a physics callback
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

}
