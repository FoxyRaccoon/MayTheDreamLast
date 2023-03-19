using Godot;
using System;

public partial class EyeBox : Area2D
{
    private bool PlayerCollision = false;
    private bool Opened = false;
    private static int BoxOpened = 0;
    public static void ResetBoxOpened(){
        BoxOpened = 0;
    }
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
            if(GetNode<PaintedZone>("PaintedZone").GetCompleteness() >= 0.95f && PlayerData.GetEyeCount() > 0 && !Opened){
                PlayerData.AddEyeCount(-1);
                GetNode<Sprite2D>("Sprite/Sprite2D").Frame = 1;
                GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
                Opened = true;
                BoxOpened++;
                if(BoxOpened == 5){
                    GetNode<World>("/root/World").DisableCollision();
                    GetParent().GetParent().GetNode<UI>("UI").ClearUIUpdate();
                    GetParent().GetParent().GetNode<Player>("Player").ClearEvents();
                    Door.ClearEvents();
                    GlobalData.Init();
                    StarBox.ResetBoxOpened();
                    EyeBox.ResetBoxOpened();
                    PlayerData.ResetItems();
                    PlayerData.DoubleStats();
                    GetNode<Transition>("/root/Transition").WhiteTransition("res://World/world.tscn");
                }
            }
        }
    }
}
