using Godot;
using System;

public partial class Gunner : PlayerInput
{
    private Node2D turretAnchor;

    [Export]
    private Node2D turret;

    private Vector2 cursorPos;

    public override void _Ready()
    {
        GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));

        if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() ==
            Multiplayer.GetUniqueId())
            camera.MakeCurrent();
    }

    public override void _Process(double delta)
    {
        Position = turretAnchor.GlobalPosition;

        if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() !=
            Multiplayer.GetUniqueId())
            return;

        FaceCursor();
        MoveCamera();
    }

    public override void _Draw()
    {
        DrawLine(Position, Transform.X * 10, Colors.Red, 1);
    }

    public override void LeftClick()
    {
        //QueueRedraw();
    }

    public void SetAnchor(Node2D anchor)
    {
        turretAnchor = anchor;
    }

    private void FaceCursor()
    {
        cursorPos = GetGlobalMousePosition();
        float angleRad = Position.AngleToPoint(cursorPos);

        Rotation = angleRad;
    }

    private void MoveCamera()
    {
        Vector2 direction = (cursorPos - turretAnchor.GlobalPosition);
        camera.GlobalPosition = turretAnchor.GlobalPosition + direction/2;
    }
}
