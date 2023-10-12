using Godot;
using System;

public partial class SceneManager : Node2D
{
	//[Export]
	//private PackedScene playerCameraScene;

	[Export]
	private PackedScene vehicleHullScene;

    [Export]
    private PackedScene vehicleTurretScene;

	[Export]
	private Node2D startPosition;

    private Driver driverPlayer;
    private Gunner gunnerPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        foreach (PlayerInfo playerInfo in GameManager.Players)
		{
            if (playerInfo.crewPosition == PlayerInfo.ECrewPosition.Driver)
			{
                driverPlayer = vehicleHullScene.Instantiate<Driver>();
                driverPlayer.Name = playerInfo.Id.ToString();
                AddChild(driverPlayer);
                driverPlayer.GlobalPosition = startPosition.GlobalPosition;
            }
			else
			{
                gunnerPlayer = vehicleTurretScene.Instantiate<Gunner>();
                gunnerPlayer.Name = playerInfo.Id.ToString();
                gunnerPlayer.SetAnchor(driverPlayer.GetNode<Node2D>("turretAnchor"));
                AddChild(gunnerPlayer);
            }
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
