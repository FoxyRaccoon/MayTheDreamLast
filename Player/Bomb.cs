using Godot;
using System;

public partial class Bomb : RigidBody2D
{
    private float Strength = 0.8f;

    public void SetStrength(float strength)
	{
		Strength = strength;
	}

    public override void _Ready()
    {
        GetNode<Explosion>("Explosion").SetStrength(Strength);
        GetNode<AnimationPlayer>("AnimationPlayer").Play("start");
    }

    public void Explode(){
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Entity/Death");
    }

    public void Die()
	{
		CallDeferred("queue_free");
	}
}
