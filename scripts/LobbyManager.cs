using Godot;
using System;

public partial class LobbyManager : Control
{
    private ENetMultiplayerPeer peer;

    private Control playerContainer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;

        playerContainer = GetNode<HBoxContainer>("player_container");
        
        if (Multiplayer.IsServer())
            Rpc(nameof(LoadPlayer));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    /*
     * =================================
     * BUTTON METHODS
     * =================================
     */

    public void _on_start_button_down()
    {
        Rpc(nameof(StartGame));
    }

    /*
     * =================================
     * CONNECTION METHODS
     * =================================
     */

    [Rpc(MultiplayerApi.RpcMode.AnyPeer,
        CallLocal = true,
        TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void LoadPlayer()
    {
        for (int i = 0; i < GameManager.Players.Count; i++)
        {
            playerContainer.GetChildren()[i].GetNode<Label>("name").Text = GameManager.Players[i].Name;
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer,
        CallLocal = true,
        TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void StartGame()
    {
        Node2D scene = ResourceLoader.Load<PackedScene>("res://level_scenes/test_scene_2d.tscn").
            Instantiate<Node2D>();
        GetTree().Root.AddChild(scene);
        Hide();

        if (!Multiplayer.IsServer())
            return;

        GD.Print("Connected Players:");
        foreach (PlayerInfo player in GameManager.Players)
        {
            GD.Print($"{player.Name}");
        }
    }

    /*
     * =================================
     * MULTIPLAYER API METHODS
     * =================================
     */

    /// <summary>
	/// Runs when a player connects and runs on all peers
	/// </summary>
	/// <param name="id"></param>
    private void PeerConnected(long id)
    {
        Rpc(nameof(LoadPlayer));
    }

    /// <summary>
    /// Runs when a player disconnects and runs on all peers
    /// </summary>
    /// <param name="id"></param>
    private void PeerDisconnected(long id)
    {
        Rpc(nameof(LoadPlayer));
    }
}
