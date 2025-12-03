using Godot;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public partial class Player : CharacterBody2D
{
	public const float Speed = 120.0f;

	AnimatedSprite2D AnimatedSprite;
	Area2D HurtBox;

	private int health = 100;

	// Tune these two:
	private float knockbackStrength = 400f;  // try 200–600 to feel it
	private float knockbackDamp = 8f;        // how quickly it slows down

	private bool isKnockedBack = false;
	private Vector2 _knockbackVelocity = Vector2.Zero;

	public override void _Ready()
	{
		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		HurtBox = GetNode<Area2D>("Hurtbox");
		HurtBox.AreaEntered += TakeDamage;
	}

	public override void _Process(double delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();
		AnimatedSprite.FlipH = mousePos.X < GlobalPosition.X;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (health <= 0) {
			Die();
			return;
		}

		Vector2 velocity = Velocity;

		if (isKnockedBack)
		{
			AnimatedSprite.SelfModulate = Colors.Red;
			// Smoothly reduce knockback velocity toward zero
			float t = knockbackDamp * (float)delta;
			t = Mathf.Clamp(t, 0f, 1f);

			_knockbackVelocity = _knockbackVelocity.Lerp(Vector2.Zero, t);

			if (_knockbackVelocity.LengthSquared() < 3f)
			{
				_knockbackVelocity = Vector2.Zero;
				AnimatedSprite.SelfModulate = Colors.White;
				isKnockedBack = false;
			}

			velocity = _knockbackVelocity;
		}
		else
		{
			Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

			if (direction != Vector2.Zero)
			{
				AnimatedSprite.Play("run");
				velocity.X = direction.X * Speed;
				velocity.Y = direction.Y * Speed;
			}
			else
			{
				AnimatedSprite.Play("idle");
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void TakeDamage(Area2D area)
	{
		if (area.IsInGroup("Enemy"))
		{
			health -= 20;
			Knockback(area);
		}
	}

	private void Knockback(Area2D enemy)
	{
		Vector2 direction = (GlobalPosition - enemy.GlobalPosition).Normalized();

		// IMPORTANT: set the knockback velocity, not Velocity directly
		_knockbackVelocity = direction * knockbackStrength;
		isKnockedBack = true;
	}

	private async void Die() 
	{
		Velocity = Vector2.Zero;
		AnimatedSprite.Play("death");
		await ToSignal(AnimatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);
		QueueFree();
	}
}
