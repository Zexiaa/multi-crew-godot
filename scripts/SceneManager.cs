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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        foreach (PlayerInfo playerInfo in GameManager.Players)
		{
			PlayerInput currentPlayer;

            if (playerInfo.crewPosition == PlayerInfo.ECrewPosition.Driver)
			{
                currentPlayer = vehicleHullScene.Instantiate<Driver>();
			}
			else
			{
                currentPlayer = vehicleTurretScene.Instantiate<PlayerInput>();
			}

            currentPlayer.Name = playerInfo.Id.ToString();
			AddChild(currentPlayer);
            currentPlayer.GlobalPosition = startPosition.GlobalPosition;
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
