using Godot;
using System;

public partial class WildJamScreen : Control
{
    public void OnTimeOut()
    {
        GetNode<Transition>("/root/Transition").BlueTransition("res://Menus/theme_screen.tscn");
    }
}
