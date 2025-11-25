using Godot;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Formats.Tar;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

public partial class Enemy1 : CharacterBody2D
{
	/*
	Stuff we want from the npc:

	- 'Activate' when the player gets to a certain radius
	- Runs towards the player once activate animation ends
	*/
	private Area2D detection_area;
	private AnimatedSprite2D sprite;
	private Boolean active = false;
	private CharacterBody2D player;
	private float speed = 50f;
	private int health = 100;
	private Area2D hitbox;

	public override void _Ready()
	{
		detection_area = GetNode<Area2D>("DetectionArea");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		hitbox = GetNode<Area2D>("Hitbox");

		detection_area.BodyEntered += OnBodyEntered;
		hitbox.AreaEntered += TakeDamage;
	}

	private async void OnBodyEntered(Node2D body)
	{
		if (!active && body.IsInGroup("player"))
		{
			player = body as CharacterBody2D;
			sprite.Play("activate");
			await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);
			sprite.Play("idle_active");
			active = true;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (health <= 0) {
			Die();
			return;
		}
		if (!active) return;
		// player following logic
		var dir = (player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = dir * speed;
		MoveAndSlide();
		sprite.FlipH = Velocity.X < 0;
		if (Velocity != Vector2.Zero)
		{
			sprite.Play("run");
		}
	}

	private void TakeDamage(Area2D area)
	{
		if (active && area.IsInGroup("Projectile"))
		{
			health -= 20;
			TurnRed();
		}
	}

	private async void TurnRed()
	{
		sprite.SelfModulate = Colors.Red;
		await ToSignal(GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
		sprite.SelfModulate = Colors.White;
	}

	private async void Die()
	{
		active = false;
		Velocity = Vector2.Zero;
		sprite.Play("death");
		await ToSignal(sprite, AnimatedSprite2D.SignalName.AnimationFinished);
		QueueFree();
	}
}
