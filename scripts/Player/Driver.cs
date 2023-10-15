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

    [NonSerialized]
    public Vector2 syncPos = Vector2.Zero;

    [NonSerialized]
    public float syncRot = 0;

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
        {
            GlobalPosition = GlobalPosition.Lerp(syncPos, SyncWeight);
            GlobalRotation = Mathf.Lerp(GlobalRotation, syncRot, SyncWeight);
            return;
        }

        velocity = Vector2.Zero;
        DetectInput();

		Position +=  velocity * (float)delta;
        syncPos = GlobalPosition;
        syncRot = GlobalRotation;
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
