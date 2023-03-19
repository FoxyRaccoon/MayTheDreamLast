using Godot;
using System;

public partial class Unicorn : CharacterBody2D
{
	private float Speed = 120.0f;
	private Random RandomNbGnrt = new Random();
	public float GetCompleteness()
	{
		return GetNode<PaintedZone>("PaintedZone").GetCompleteness();
	}

	public override void _Ready()
	{
		GetNode<NavigationAgent2D>("NavigationAgent2D").TargetPosition = GetParent().GetParent().GetNode<Player>("Player").GlobalPosition;
		GetNode<Timer>("Timer2").Start(RandomNbGnrt.NextDouble() * 5.0f);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GetCompleteness() <= 0.01f){
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
			GetNode<AnimationPlayer>("SpriteAnimation").Play("walk");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
			GetNode<AnimationPlayer>("SpriteAnimation").Play("idle");
		}
		
		if(velocity.X > 0){
			GetNode<Sprite2D>("Sprite/Sprite2D").FlipH = false;
			LookAt(GlobalPosition + velocity);
		}else if(velocity.X < 0){
			GetNode<Sprite2D>("Sprite/Sprite2D").FlipH = true;
			LookAt(GlobalPosition - velocity);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void Die()
	{
		PlayerData.AddUnicornCount(-1);
		GetNode<CollisionShape2D>("PaintedZone/CollisionShape2D").SetDeferred("disabled", true);
		CallDeferred("queue_free");
	}

	public void OnTimeOut(){
		
		GetNode<NavigationAgent2D>("NavigationAgent2D").TargetPosition = GetParent().GetParent().GetNode<Player>("Player").GlobalPosition;// + new Vector2(RandomNbGnrt.Next(-40,40), RandomNbGnrt.Next(-40,40));;
	}

	public void OnAudioFinished()
	{
		GetNode<Timer>("Timer2").Start(RandomNbGnrt.NextDouble() * 5.0f + 15.0f);
	}

	public void OnTimeOut2()
	{
		GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
	}
}
