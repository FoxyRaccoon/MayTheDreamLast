using Godot;
using System;

public partial class Nightmare : CharacterBody2D
{
	private float Speed = 100.0f;
	private Random RandomNbGnrt = new Random();
	private DoorType MalusType;
	public float GetCompleteness()
	{
		return GetNode<PaintedZone>("PaintedZone").GetCompleteness();
	}

	public override void _Ready()
	{
		GetNode<NavigationAgent2D>("NavigationAgent2D").TargetPosition = GetParent().GetParent().GetNode<Player>("Player").GlobalPosition;
		Random random = new Random();
        MalusType = (DoorType)random.Next(0, (int)DoorType.Length);
		GetNode<Timer>("Timer2").Start(RandomNbGnrt.NextDouble() * 10.0f);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GetCompleteness() <= 0.01f)
		{
			if(GetNode<PaintedZone>("PaintedZone").IsBeingPainted()){
				GetNode<AnimationPlayer>("AnimationPlayer").Play("Entity/Death");
			}else{
				Die();
			}
		}
		float speed = Speed;
		if(GetNode<PaintedZone>("PaintedZone").IsBeingPainted()){
			speed = Speed * 0.4f;
		}

		Vector2 velocity = Velocity;
		NavigationAgent2D navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		if (navAgent.IsNavigationFinished() == false)
		{
			Vector2 direction = GlobalPosition.DirectionTo(navAgent.GetNextPathPosition());
			velocity = direction * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
		}
		if(velocity.X > 0){
			GetNode<Sprite2D>("Sprite/Sprite2D").FlipH = true;
			GetNode<Node2D>("Sprite").LookAt(GlobalPosition + velocity);
		}else if(velocity.X < 0){
			GetNode<Sprite2D>("Sprite/Sprite2D").FlipH = false;
			GetNode<Node2D>("Sprite").LookAt(GlobalPosition - velocity);
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}

	public void Die()
	{
		GetNode<CollisionShape2D>("PaintedZone/CollisionShape2D").SetDeferred("disabled", true);
		CallDeferred("queue_free");
	}

	public void OnTimeOut(){
		
		GetNode<NavigationAgent2D>("NavigationAgent2D").TargetPosition = GetParent().GetParent().GetNode<Player>("Player").GlobalPosition;
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			switch (MalusType)
            {
                case DoorType.UnicornCount:
                    PlayerData.AddUnicornCount(-1*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Value/Label").Text = String.Format("{0, 0:N0}", 1*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Effect/Label").Text = "-";
					GetNode<Sprite2D>("MalusDisplay/Sprite2D").Frame = 7;
                    break;
                case DoorType.BombCount:
                    PlayerData.AddBombCount(-2*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Value/Label").Text = String.Format("{0, 0:N0}", 2*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Effect/Label").Text = "-";
					GetNode<Sprite2D>("MalusDisplay/Sprite2D").Frame = 0;
                    break;
                case DoorType.TowerCount:
                    PlayerData.AddTowerCount(-1*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Value/Label").Text = String.Format("{0, 0:N0}", 3*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Effect/Label").Text = "-";
					GetNode<Sprite2D>("MalusDisplay/Sprite2D").Frame = 2;
                    break;
                case DoorType.MaxCompleteness:
                    PlayerData.AddMaxCompleteness(-0.2f*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Value/Label").Text = String.Format("{0, 0:N3}", 0.2*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Effect/Label").Text = "▼";
					GetNode<Sprite2D>("MalusDisplay/Sprite2D").Frame = 4;
                    break;
                case DoorType.Speed:
                    PlayerData.AddSpeed(-2f*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Value/Label").Text = String.Format("{0, 0:N0}", 2*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Effect/Label").Text = "▼";
					GetNode<Sprite2D>("MalusDisplay/Sprite2D").Frame = 5;
                    break;
                case DoorType.WeaponStrength:
                    PlayerData.AddWeaponStrength(-0.01f*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Value/Label").Text = String.Format("{0, 0:N3}", 0.01*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Effect/Label").Text = "▼";
					GetNode<Sprite2D>("MalusDisplay/Sprite2D").Frame = 3;
                    break;
                case DoorType.BombStrength:
                    PlayerData.AddBombStrength(-0.01f*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Value/Label").Text = String.Format("{0, 0:N3}", 0.01*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Effect/Label").Text = "▼";
					GetNode<Sprite2D>("MalusDisplay/Sprite2D").Frame = 0;
                    break;
                case DoorType.TowerStrength:
                    PlayerData.AddTowerStrength(-0.005f*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Value/Label").Text = String.Format("{0, 0:N3}", 0.005*GlobalData.GetTier());
					GetNode<Label>("MalusDisplay/Effect/Label").Text = "▼";
					GetNode<Sprite2D>("MalusDisplay/Sprite2D").Frame = 2;
                    break;
            }
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Curse");
		}
	}

	public void OnAudioFinished()
	{
		GetNode<Timer>("Timer2").Start(RandomNbGnrt.NextDouble() * 10.0f + 5.0f);
	}

	public void OnTimeOut2()
	{
		GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
	}
}