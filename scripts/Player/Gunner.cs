using Godot;
using System;

public partial class Gunner : PlayerInput
{

    private Node2D hull;
    private Vector2 cursorPos;

    public override void _Ready()
    {
        GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
    }

    public override void _Process(double delta)
    {
        Position = hull.Position;

        if (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() !=
            Multiplayer.GetUniqueId())
            return;

        FaceCursor();
    }

    public override void _Draw()
    {
        DrawLine(Position, Transform.X * 10, Colors.Red, 1);
    }

    public override void LeftClick()
    {
        //QueueRedraw();
    }

    public void SetHull(Node2D _hull)
    {
        hull = _hull;
    }

    private void FaceCursor()
    {
        cursorPos = GetGlobalMousePosition();
        float angleRad = Position.AngleToPoint(cursorPos);

        Rotation = angleRad;
    }
}
