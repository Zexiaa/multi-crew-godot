using Godot;
using System;

public partial class PlayerInput : Node2D
{
    [Export]
    protected Camera2D camera;

    protected void DetectInput()
    {
        if (Input.IsActionPressed("move_forward"))
            MoveForward();

        if (Input.IsActionPressed("move_backward"))
            MoveBackward();

        if (Input.IsActionPressed("move_right"))
            MoveRight();

        if (Input.IsActionPressed("move_left"))
            MoveLeft();

        if (Input.IsActionPressed("left_click"))
            LeftClick();
    }

    public virtual void MoveForward()
    {

    }

    public virtual void MoveBackward()
    {

    }

    public virtual void MoveLeft()
    {

    }

    public virtual void MoveRight()
    {

    }

    public virtual void LeftClick()
    {

    }
}
