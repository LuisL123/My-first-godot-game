using Godot;
using Godot.NativeInterop;

public partial class Bullet : Area2D
{
	[Export] public float Speed = 150f;
	[Export] public float Life = 1.25f;

	private Vector2 _dir = Vector2.Right;
	private AnimatedSprite2D _sprite;

	public void Fire(Vector2 direction)
	{
		_dir = direction.Normalized();
		Rotation = _dir.Angle();
	}

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");  // get sprite first

		var t = GetTree().CreateTimer(Life);
		t.Timeout += OnLifeEndedAsync;
	}

	private async void OnLifeEndedAsync()
	{
		// stop moving while splashing
		_dir = Vector2.Zero;
		SetPhysicsProcess(false);
		Rotation = _dir.Angle();
		_sprite.Play("splash");
		await ToSignal(_sprite, AnimatedSprite2D.SignalName.AnimationFinished);
		QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (HasOverlappingBodies())
		{
			OnLifeEndedAsync();
		}
		Position += _dir * Speed * (float)delta;
	}
}
