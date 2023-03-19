using Godot;
using System;

public partial class ScoreLine : HBoxContainer
{
    public void SetScore(string name, float score)
    {
        GetNode<Label>("Name").Text = name;
        GetNode<Label>("Score").Text = score.ToString();
    }
}
