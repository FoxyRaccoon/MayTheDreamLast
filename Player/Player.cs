using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private float MaxCompleteness = 400.0f;
	private float Completeness;
	private PackedScene BombScene;
	private PackedScene TowerScene;
	private PackedScene UnicornScene;
	private CollisionShape2D Collision;
	private LightOccluder2D Occluder;

	public override void _Ready()
	{
		BombScene = (PackedScene)ResourceLoader.Load("res://Player/bomb.tscn");
		TowerScene = (PackedScene)ResourceLoader.Load("res://Player/tower.tscn");
		UnicornScene = (PackedScene)ResourceLoader.Load("res://Friends/unicorn.tscn");
		Collision = GetNode<CollisionShape2D>("CollisionShape2D");
		Occluder = GetNode<LightOccluder2D>("LightOccluder2D");
		Initialisation();
	}
	public void Initialisation(){
		Random random = new Random();
		Completeness = MaxCompleteness;
		if(GetParent() is World){
			for(int i = 0; i < PlayerData.GetUnicornCount(); i++){
				Unicorn unicorn = (Unicorn)UnicornScene.Instantiate();
				unicorn.GlobalPosition = GlobalPosition + new Vector2(random.Next(-80,80), random.Next(-80,80));
				GetParent().GetNode("Entities").CallDeferred("add_child", unicorn);
			}
		}else if(GetParent() is GameMenu){
			GetNode<AnimationPlayer>("AnimationPlayer").Play("zoom");
		}
		
		GetNode<Weapon>("WeaponOrigin/Weapon").SetStrength(PlayerData.GetWeaponStrength());
		PlayerData.SpeedChanged += OnSpeedChanged;
		PlayerData.WeaponStrengthChanged += OnWeaponStrengthChanged;
		PlayerData.ItemCountChanged += OnItemCountChanged;
		OnSpeedChanged();
		OnWeaponStrengthChanged();
		OnItemCountChanged();
	}
	public override void _PhysicsProcess(double delta)
	{
		bool isAttacking = Input.IsActionPressed("attack");
		bool isAttackingNegative = Input.IsActionPressed("attack_negative");
		bool isSendingBomb = Input.IsActionJustPressed("send_bomb");
		bool isPutingTower = Input.IsActionJustPressed("put_tower");
		if (isAttacking){
			GetNode<Weapon>("WeaponOrigin/Weapon").Attack();
		}else if(isAttackingNegative){
			GetNode<Weapon>("WeaponOrigin/Weapon").NegativeAttack();
		}else
		{
			GetNode<Weapon>("WeaponOrigin/Weapon").StopAttack();
		}
		GetNode<Marker2D>("WeaponOrigin").LookAt(GetGlobalMousePosition());
		if(GlobalPosition.DirectionTo(GetGlobalMousePosition()).X > 0){
			// GetNode<Sprite2D>("WeaponOrigin/Weapon/Sprite2D").FlipH = false;
			GetNode<Sprite2D>("WeaponOrigin/Weapon/Sprite2D").FlipV = false;
			
			
		}else{
			// GetNode<Sprite2D>("WeaponOrigin/Weapon/Sprite2D").FlipH = true;
			GetNode<Sprite2D>("WeaponOrigin/Weapon/Sprite2D").FlipV = true;
		}

		if(isSendingBomb && PlayerData.GetBombCount() > 0){
			Bomb bomb = (Bomb)BombScene.Instantiate();
			bomb.SetStrength(PlayerData.GetBombStrength());
			GetNode<Marker2D>("BombOrigin").LookAt(GetGlobalMousePosition());
			bomb.GlobalPosition = GetNode<Marker2D>("BombOrigin/BombPoint").GlobalPosition;
			bomb.LinearVelocity = GlobalPosition.DirectionTo(GetGlobalMousePosition()) * 200.0f;
			GetParent().AddChild(bomb);
			PlayerData.AddBombCount(-1);
		}

		if(isPutingTower && PlayerData.GetTowerCount() > 0){
			Tower tower = (Tower)TowerScene.Instantiate();
			tower.SetStrength(PlayerData.GetTowerStrength());
			tower.GlobalPosition = GetGlobalMousePosition();
			GetParent().AddChild(tower);
			PlayerData.AddTowerCount(-1);
		}
		
		float totalNegativeCompleteness = 0.0f;
		float currentNegativeCompleteness = 0.0f;
		float currentPositiveCompleteness = 0.0f;
		foreach(Area2D area in GetNode<Area2D>("Area2D").GetOverlappingAreas()){
			if(area.GetParent() is Wall){
				totalNegativeCompleteness += 1.0f;
				currentNegativeCompleteness += area.GetParent<Wall>().GetCompleteness();
			}else if(area.GetParent() is Unicorn){
				currentPositiveCompleteness += area.GetParent<Unicorn>().GetCompleteness();
			}
		}
		Completeness += (float)(currentPositiveCompleteness +(currentNegativeCompleteness - totalNegativeCompleteness)*(GlobalData.GetDifficulty()*0.5f*delta));
		if(Completeness <= 0.0f){
			Completeness = 0.0f;
			//Wake up
		}else if(Completeness >= MaxCompleteness){
			Completeness = MaxCompleteness;
		}

		Vector2 velocity = Velocity;
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity = direction * PlayerData.GetSpeed();
			if(isAttacking || isAttackingNegative){
				velocity *= 0.7f;
			}
			GetNode<AnimationPlayer>("SpriteAnimation").Play("move");
			if(direction.X > 0){
				GetNode<Marker2D>("Marker2D").Scale = new Vector2(1,1);
			}else if(direction.X < 0){
				GetNode<Marker2D>("Marker2D").Scale = new Vector2(-1,1);
			}

		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, PlayerData.GetSpeed());
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, PlayerData.GetSpeed());
			GetNode<AnimationPlayer>("SpriteAnimation").Play("idle");
		}

		Velocity = velocity;
		MoveAndSlide();

		Modulate = new Color(1.0f, 1.0f, 1.0f, Completeness/MaxCompleteness);

		if(Completeness <= 0.4){
			Collision.Disabled = true;
			Occluder.Visible = false;
		}else{
			Collision.Disabled = false;
			Occluder.Visible = true;
		}
		if (Completeness <= 0.01f){
			WakeUp();
		}
	}

	public void WakeUp(){
		GetNode<Transition>("/root/Transition").BlackTransition("res://Menus/end_menu.tscn");
		GetParent().GetNode<UI>("UI").ClearUIUpdate();
        ClearEvents();
		Door.ClearEvents();
	}

	public void OnSpeedChanged(){
		if(IsInstanceValid(this)){
			float bluePercentage = Math.Max(Math.Min(1.0f, (PlayerData.GetSpeed()-140f)/80f), 0f);
			((ParticleProcessMaterial)GetNode<GpuParticles2D>("GPUParticles2D").ProcessMaterial).Color = new Color(1f, 0, 1f)*(1-bluePercentage) + new Color(0f, 1f, 1f)*bluePercentage;
		}
	}

	public void OnWeaponStrengthChanged(){
		if(IsInstanceValid(this)){
		int weaponState = (int)Math.Truncate(Math.Max((PlayerData.GetWeaponStrength() - 0.3f)/0.1f, 0f));
			switch(weaponState){
				case 0:
					GetNode<Sprite2D>("WeaponOrigin/Weapon/Sprite2D").SetDeferred("frame", 2);
					break;
				case 1:
					GetNode<Sprite2D>("WeaponOrigin/Weapon/Sprite2D").SetDeferred("frame", 4);
					break;
				case 2:
					GetNode<Sprite2D>("WeaponOrigin/Weapon/Sprite2D").SetDeferred("frame", 3);
					break;
				case 3:
					GetNode<Sprite2D>("WeaponOrigin/Weapon/Sprite2D").SetDeferred("frame", 1);
					break;
				default:
					GetNode<Sprite2D>("WeaponOrigin/Weapon/Sprite2D").SetDeferred("frame", 0);
					break;
			}
		}
		
	}

	public void OnItemCountChanged(){
		if(IsInstanceValid(this)){
			if(PlayerData.GetStarCount() > 0){
				GetNode<GpuParticles2D>("Stars").Emitting = true;
				GetNode<GpuParticles2D>("Stars").Amount = PlayerData.GetStarCount();
			}else{
				GetNode<GpuParticles2D>("Stars").Emitting = false;
			}
			if(PlayerData.GetEyeCount() > 0){
				GetNode<GpuParticles2D>("Eyes").Emitting = true;
				GetNode<GpuParticles2D>("Eyes").Amount = PlayerData.GetEyeCount();
			}else{
				GetNode<GpuParticles2D>("Eyes").Emitting = false;
			}
		}
	}

	public void ClearEvents(){
		PlayerData.SpeedChanged -= OnSpeedChanged;
		PlayerData.WeaponStrengthChanged -= OnWeaponStrengthChanged;
		PlayerData.ItemCountChanged -= OnItemCountChanged;
	}
	
}
