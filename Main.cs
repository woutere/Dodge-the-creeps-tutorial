using Godot;
using System;

public partial class Main : Node
{

	[Export]
	public PackedScene MobScene {get; set;}
	private int _score;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
	}

	public void NewGame()
	{
		_score = 0;

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
	}

	private void OnScoreTimerTimeout()
	{
		_score++;
	}

	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	private void OnMobTimerTimeout()
	{
		//Create a new instance of a mob Scene
		Mob mob = MobScene.Instantiate<Mob>();

		//Chooses a random location on path2D
		var mobSpawnLocation = GetNode<PathFollow2D>("Mobpath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		//Set the mob perpendicular to path's direction
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Set the mob position to a random location
		mob.Position = mobSpawnLocation.Position;

		//Add some randomness
		direction += (float)GD.RandRange(-MathF.PI / 4, Mathf.Pi/4);
		mob.Rotation = direction;

		//choose velocity
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0),0);
		mob.LinearVelocity = velocity.Rotated(direction);

		//spawn the mob into main scene
		AddChild(mob);
	}

}
