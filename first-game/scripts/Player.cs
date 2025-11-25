using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 120.0f;

	//@onready animated_sprite_2d AnimatedSprite = $AnimatedSprite2D;
	AnimatedSprite2D AnimatedSprite;

	public override void _Ready()
	{
		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _Process(double delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();
		AnimatedSprite.FlipH = mousePos.X < GlobalPosition.X;

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		if (direction != Vector2.Zero) {
			AnimatedSprite.Play("run");
		} else {
			AnimatedSprite.Play("idle");
		}


		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
