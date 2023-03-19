using Godot;
using System;

public partial class UI : Control
{
    private Label InceptionLabel;
    private Label TierLabel;
    private Label OVNILabel;
    private Label BombLabel;
    public override void _Ready()
    {
        InceptionLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Inception");
        TierLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Tier");
        OVNILabel = GetNode<Label>("VBoxContainer/HBoxContainer2/OVNI");
        BombLabel = GetNode<Label>("VBoxContainer/HBoxContainer2/Bomb");
        GlobalData.UIUpdate += UpdateUI;
        PlayerData.UIUpdate += UpdateUI;
        UpdateUI();
    }

    public void ClearUIUpdate(){
        GlobalData.UIUpdate -= UpdateUI;
        PlayerData.UIUpdate -= UpdateUI;
    }

    public void UpdateUI(){
        InceptionLabel.Text = GlobalData.GetDifficulty().ToString();
        TierLabel.Text = GlobalData.GetTier().ToString();
        OVNILabel.Text = PlayerData.GetTowerCount().ToString();
        BombLabel.Text = PlayerData.GetBombCount().ToString();
    }
}
