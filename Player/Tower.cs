using Godot;
using System;

public partial class Tower : StaticBody2D
{   
    private float Strength = 0.1f;

    public void SetStrength(float strength)
	{
		Strength = strength;
	}

    public float GetCompleteness()
	{
		return GetNode<PaintedZone>("PaintedZone").GetCompleteness();
	}
	public float GetStrength()
	{
		return Strength * GetCompleteness();
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<PaintedZone>("PaintedZone").AddAreaToExclude(GetNode<PaintingZone>("PaintingZone"));
        ((ParticleProcessMaterial)GetNode<GpuParticles2D>("GPUParticles2D").ProcessMaterial).Color = new Color(0f, 0.9f, 1f, Strength);
        GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
    }

    public override void _PhysicsProcess(double delta)
	{
        GetNode<PaintingZone>("PaintingZone").SetStrength(GetStrength());
        if(GetCompleteness() <= 0.01f){
            GetNode<CollisionShape2D>("PaintingZone/CollisionShape2D").Disabled = true;
            CallDeferred("queue_free");
        }
	}

    public void OnAudioFinished(){
		GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
	}
}
