using Godot;

public partial class MultiplayerController : Control
{
	private string address = "127.0.0.1";
	private int port = 54337;

	private ENetMultiplayerPeer peer;

    private int maxPlayers = 4;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;
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

    public void _on_host_button_down()
    {
        peer = new ENetMultiplayerPeer();
        Error error = peer.CreateServer(port, maxPlayers);

        if (error != Error.Ok)
        {
            GD.Print("Error cannot host on " + error.ToString());
            return;
        }

        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = peer;
        SendPlayerInfo(GetNode<LineEdit>("name_textbox").Text, 1);

        GD.Print("Waiting For Players...");
    }

    public void _on_join_button_down()
    {
        peer = new ENetMultiplayerPeer();
        peer.CreateClient(address, port);

        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = peer;

        GD.Print("Joining Game!");
    }

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
            GD.Print($"Of {player.crewPosition}");
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void SendPlayerInfo(string name, int id)
    {
        PlayerInfo info = new PlayerInfo(name, id);

        if (!GameManager.Players.Contains(info))
            GameManager.Players.Add(info);
    
        if (Multiplayer.IsServer())
        {
            foreach (PlayerInfo player in GameManager.Players)
            {
                Rpc(nameof(SendPlayerInfo), player.Name, player.Id);
            }
        }
    }

    /*
     * =================================
     * MULTIPLAYER API METHODS
     * =================================
     */

    /// <summary>
    /// Runs when connection fails and only runs on client
    /// </summary>
    private void ConnectionFailed()
    {
		GD.Print("CONNECTION FAILED!");
    }

	/// <summary>
	/// Runs when connection is successful and only runs on client
	/// </summary>
    private void ConnectedToServer()
    {
		GD.Print("CONNECTED!");

        RpcId(1, nameof(SendPlayerInfo), GetNode<LineEdit>("name_textbox").Text, Multiplayer.GetUniqueId());
    }

    /// <summary>
    /// Runs when a player connects and runs on all peers
    /// </summary>
    /// <param name="id"></param>
    private void PeerConnected(long id)
    {
        GD.Print($"Player of ID {id} connected");
    }

    /// <summary>
    /// Runs when a player disconnects and runs on all peers
    /// </summary>
    /// <param name="id"></param>
    private void PeerDisconnected(long id)
    {
        GD.Print($"Player of ID {id} disconnected");
    }
}
