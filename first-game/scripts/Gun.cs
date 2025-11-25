using Godot;
using System;
using System.Runtime.ExceptionServices;

public partial class Gun : Node2D
{
	Sprite2D sprite;
	Marker2D muzzle;
	PackedScene bullet_path;
	Boolean can_shoot = true;
	float firerate = 5f;
	Timer timer;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		muzzle = GetNode<Marker2D>("Muzzle");
		bullet_path = GD.Load<PackedScene>("res://scenes/Items/bullet.tscn");
		timer = GetNode<Timer>("Timer");
		timer.Timeout += () => can_shoot = true;
	}

	public override void _Process(double delta){
		Vector2 mousePos = GetGlobalMousePosition();
		LookAt(mousePos);
		bool isMouseLeft = mousePos.X < GlobalPosition.X;
		sprite.FlipV = isMouseLeft;
		Console.WriteLine(can_shoot);
		if (Input.IsActionPressed("shoot") && can_shoot) {
			Shoot();
		}
	}

	private void Shoot() {
		var bullet = bullet_path.Instantiate<Bullet>();
		GetTree().CurrentScene.AddChild(bullet);
		bullet.GlobalPosition = muzzle.GlobalPosition;
		Vector2 mouse = GetGlobalMousePosition();
		Vector2 dir = mouse - muzzle.GlobalPosition;
		bullet.Fire(dir);
		// shooting cooldown to reduce rate of fire
		can_shoot = false;
		timer.Start(1f / firerate);
	}
}
