using Godot;
using System;
public partial class PhysicsShell : StaticBody2D
{
	[Export]
	public float bulletSpeed = 10.0f;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		//ApplyForce(Transform.X * bulletSpeed * (float)delta);

		Translate(Transform.X * bulletSpeed * (float)delta);
	}
}
