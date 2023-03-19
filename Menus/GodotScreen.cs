using Godot;
using System;

public partial class GodotScreen : Control
{
    public override void _Ready()
    {
        GetNode<MusicListener>("/root/MusicListener").Stop();
    }

    public void OnTimeOut()
    {
        GetNode<Transition>("/root/Transition").BlueTransition("res://Menus/wild_jam_screen.tscn");
    }
}
