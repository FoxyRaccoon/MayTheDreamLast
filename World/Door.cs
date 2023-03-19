using Godot;
using System;
using System.Collections.Generic;

public partial class Door : Area2D
{
    private DoorType DoorType;
    private bool NightmareLevel = false;
    private static List<Door> Doors = new List<Door>();
    public DoorType GetDoorType()
    {
        return DoorType;
    }
    public bool GetNightmareLevel()
    {
        return NightmareLevel;
    }
    public override void _Ready()
    {
        if(!(GetParent() is GameMenu))
        {
            Doors.Add(this);
            GlobalData.UIUpdate += UpdateUI;
            Random random = new Random();
            DoorType = (DoorType)random.Next(0, (int)DoorType.Length);
            NightmareLevel = random.Next(0, 100) < 5 + GlobalData.GetDifficulty() + (GlobalData.GetNightmareLevel() ? 40 : 0);
            if (NightmareLevel)
            {
                GetNode<GpuParticles2D>("GPUParticles2D").Modulate = new Color(0.8f, 0.5f, 0.5f);
            }

            switch (DoorType)
            {
                case DoorType.UnicornCount:
                    GetNode<Sprite2D>("Sprite2D").Frame = 7;
                    break;
                case DoorType.BombCount:
                    GetNode<Sprite2D>("Sprite2D").Frame = 0;
                    break;
                case DoorType.TowerCount:
                    GetNode<Sprite2D>("Sprite2D").Frame = 2;
                    break;
                case DoorType.MaxCompleteness:
                    GetNode<Sprite2D>("Sprite2D").Frame = 4;
                    break;
                case DoorType.Speed:
                    GetNode<Sprite2D>("Sprite2D").Frame = 5;
                    break;
                case DoorType.WeaponStrength:
                    GetNode<Sprite2D>("Sprite2D").Frame = 3;
                    break;
                case DoorType.BombStrength:
                    GetNode<Sprite2D>("Sprite2D").Frame = 0;
                    break;
                case DoorType.TowerStrength:
                    GetNode<Sprite2D>("Sprite2D").Frame = 2;
                    break;
            }
        }else{
            GetNode<Label>("Value/Label").Text = "";
            GetNode<Label>("Effect/Label").Text = "";
            GetNode<Sprite2D>("Sprite2D").Visible = false;
        }
    }
    public void UpdateUI()
    {
        if(!(GetParent() is GameMenu) && IsInstanceValid(this))
        {
            int multiplier = GlobalData.GetTier() * (GetNightmareLevel() ? 2 : 1);
            switch (DoorType)
            {
                case DoorType.UnicornCount:
                    GetNode<Label>("Value/Label").Text = String.Format("{0, 0:N0}", 1*multiplier);
                    GetNode<Label>("Effect/Label").Text = "+";
                    break;
                case DoorType.BombCount:
                    GetNode<Label>("Value/Label").Text = String.Format("{0, 0:N0}", 2*multiplier);
                    GetNode<Label>("Effect/Label").Text = "+";
                    break;
                case DoorType.TowerCount:
                    GetNode<Label>("Value/Label").Text = String.Format("{0, 0:N0}", 1*multiplier);
                    GetNode<Label>("Effect/Label").Text = "+";
                    break;
                case DoorType.MaxCompleteness:
                    GetNode<Label>("Value/Label").Text = String.Format("{0, 0:N3}", 0.2f*multiplier);
                    GetNode<Label>("Effect/Label").Text = "▲";
                    break;
                case DoorType.Speed:
                    GetNode<Label>("Value/Label").Text = String.Format("{0, 0:N0}", 2f*multiplier);
                    GetNode<Label>("Effect/Label").Text = "▲";
                    break;
                case DoorType.WeaponStrength:
                    GetNode<Label>("Value/Label").Text = String.Format("{0, 0:N3}", 0.01f*multiplier);
                    GetNode<Label>("Effect/Label").Text = "▲";
                    break;
                case DoorType.BombStrength:
                    GetNode<Label>("Value/Label").Text = String.Format("{0, 0:N3}", 0.01f*multiplier);
                    GetNode<Label>("Effect/Label").Text = "▲";
                    break;
                case DoorType.TowerStrength:
                    GetNode<Label>("Value/Label").Text = String.Format("{0, 0:N3}", 0.005f*multiplier);
                    GetNode<Label>("Effect/Label").Text = "▲";
                    break;
            }
        }
    }

    public static void ClearEvents()
    {
        foreach(Door door in Doors)
        {
            GlobalData.UIUpdate -= door.UpdateUI;
        }
        Doors.Clear();
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player){   
            if(!(GetParent() is GameMenu)){
                GetNode<World>("/root/World").DisableCollision();
                int multiplier = GlobalData.GetTier() * (GetNightmareLevel() ? 2 : 1);
                switch (DoorType)
                {
                    case DoorType.UnicornCount:
                        PlayerData.AddUnicornCount(1*multiplier);
                        break;
                    case DoorType.BombCount:
                        PlayerData.AddBombCount(2*multiplier);
                        break;
                    case DoorType.TowerCount:
                        PlayerData.AddTowerCount(1*multiplier);
                        break;
                    case DoorType.MaxCompleteness:
                        PlayerData.AddMaxCompleteness(0.2f*multiplier);
                        break;
                    case DoorType.Speed:
                        PlayerData.AddSpeed(2f*multiplier);
                        break;
                    case DoorType.WeaponStrength:
                        PlayerData.AddWeaponStrength(0.01f*multiplier);
                        break;
                    case DoorType.BombStrength:
                        PlayerData.AddBombStrength(0.01f*multiplier);
                        break;
                    case DoorType.TowerStrength:
                        PlayerData.AddTowerStrength(0.005f*multiplier);
                        break;
                }
                if(NightmareLevel)
                {
                    GlobalData.SetNightmareLevel(true);
                }
                else
                {
                    GlobalData.SetNightmareLevel(false);
                }
                GlobalData.SetDifficulty(GlobalData.GetDifficulty() + 1);
                GetParent().GetParent().GetNode<UI>("UI").ClearUIUpdate();
                GetParent().GetParent().GetNode<Player>("Player").ClearEvents();
                StarBox.ResetBoxOpened();
                EyeBox.ResetBoxOpened();
                ClearEvents();
            }else{
                PlayerData.Init();
                GlobalData.Init();
                StarBox.ResetBoxOpened();
                EyeBox.ResetBoxOpened();
            }
            GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
            GetNode<Transition>("/root/Transition").WhiteTransition("res://World/world.tscn");
        }
    }
}
