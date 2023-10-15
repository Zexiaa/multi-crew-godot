using Godot;
using System;

public partial class Gunner : PlayerInput
{
    [ExportGroup("Objects")]

    private Node2D turretAnchor;

    [Export]
    private Node2D turret;

    [Export]
    private Node2D exitPoint;

    [Export]
    private PackedScene shellScene;

    private Vector2 cursorPos;

    [NonSerialized]
    public float syncRot = 0;

    [ExportGroup("Attack Attr")]

    [Export]
    private float reloadTime = 2.0f;

    private Timer timer;
    private bool isReloaded = true;


    public override void _Ready()
    {
        GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));

        if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
            Multiplayer.GetUniqueId())
        {
            camera.MakeCurrent();
            timer = new Timer();
            AddChild(timer);
            timer.WaitTime = reloadTime;
            timer.Connect("timeout", new Callable(this, MethodName.OnTimerTimeout));
        }
    }

    public override void _Process(double delta)
    {
        Position = turretAnchor.GlobalPosition;

        if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() !=
            Multiplayer.GetUniqueId())
        {
            Rotation = Mathf.Lerp(Rotation, syncRot, SyncWeight);
            return;
        }

        DetectInput();

        FaceCursor();
        MoveCamera();
    }

    public override void LeftClick()
    {
        if (!isReloaded) return;

        Rpc(nameof(Fire));

        isReloaded = false;
        timer.Start();
    }

    public void SetAnchor(Node2D anchor)
    {
        turretAnchor = anchor;
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer,
        CallLocal = true)]
    private void Fire()
    {
        PhysicsShell shell = shellScene.Instantiate<PhysicsShell>();
        shell.GlobalPosition = exitPoint.GlobalPosition;
        shell.Rotation = Rotation;
        GetTree().Root.AddChild(shell);
    }

    private void FaceCursor()
    {
        cursorPos = GetGlobalMousePosition();
        float angleRad = Position.AngleToPoint(cursorPos);

        Rotation = angleRad;
        syncRot = angleRad;
    }

    private void MoveCamera()
    {
        Vector2 direction = (cursorPos - turretAnchor.GlobalPosition);
        camera.GlobalPosition = turretAnchor.GlobalPosition + direction/2;
    }

    private void OnTimerTimeout()
    {
        isReloaded = true;
    }
}
