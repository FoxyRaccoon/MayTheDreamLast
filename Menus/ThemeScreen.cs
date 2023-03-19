using Godot;
using System;

public partial class ThemeScreen : Control
{
    public void OnTimeOut()
    {
        GetNode<Transition>("/root/Transition").BlueTransition("res://Menus/logo_screen.tscn");
    }
}
