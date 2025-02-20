using Godot;
using System;

public partial class main : Node
{
	[Export]
	public PackedScene MobScene {get; set;}
	private int _score;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void NewGame()
	{
		_score = 0;
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
			
		Player player = GetNode<Player>("Player");
		Marker2D startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		
		GetNode<Timer>("StartTimer").Start();
		
		HUD hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
	}

	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		
		GetNode<HUD>("HUD").ShowGameOver();
	}
	
	private void OnMobTimerTimeout()
	{
		// Create a new instance of the Mob scene
		Mob mob = MobScene.Instantiate<Mob>();
		
		// Choose a random location on Path2D
		PathFollow2D mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		
		// Set the mob's direction perpendicular to the path direction
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
		
		// Set the mob's position to a random location
		mob.Position = mobSpawnLocation.Position;
		
		// Add some randomness to the direction
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;
		
		// Choose the velocity
		Vector2 velocity = new Vector2( (float)GD.RandRange(150.0, 250.0), 0 );
		mob.LinearVelocity = velocity.Rotated(direction);
		
		// Spawn the mob by adding it to the main scene
		AddChild(mob);
		
	}
	
	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}
	
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}
	
}

