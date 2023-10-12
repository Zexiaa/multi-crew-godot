using Godot;
using System;

public partial class Driver : PlayerInput
{
	[Signal]
	public delegate void HitEventHandler();

	[Export]
	private int speed = 400;

    [Export]
    private float rotationSpeed = 10.0f;

	private Vector2 velocity;

    public override void _Ready()
    {
        GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));

        if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
            Multiplayer.GetUniqueId())
            camera.MakeCurrent();
    }

    public override void _Process(double delta)
    {
        if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() !=
            Multiplayer.GetUniqueId())
            return;

        velocity = Vector2.Zero;
        DetectInput();

		Position +=  velocity * (float)delta;
    }

    public override void MoveForward()
    {
        velocity += Transform.X * speed;
    }

    public override void MoveBackward()
    {
        velocity -= Transform.X * speed;
    }

    public override void MoveLeft()
    {
        RotationDegrees -= rotationSpeed;
    }

    public override void MoveRight()
    {
        RotationDegrees += rotationSpeed;
    }

	private void OnBodyEntered(PhysicsBody2D body)
	{
		EmitSignal(SignalName.Hit);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
}
