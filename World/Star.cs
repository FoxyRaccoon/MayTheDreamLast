using Godot;
using System;

public partial class Star : Node2D
{
    private bool PlayerCollision = false;
    public void OnBodyEntered(Node body)
    {
        if (body is Player)
        {
            PlayerCollision = true;
        }
    }

    public void OnBodyExited(Node body)
    {
        if (body is Player)
        {
            PlayerCollision = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(PlayerCollision){
            if(GetNode<PaintedZone>("PaintedZone").GetCompleteness() >= 0.95f){
                PlayerData.AddStarCount(1);
                QueueFree();
            }
        }
    }

    public void OnAudioStreamFinished()
    {
        GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
    }
}
