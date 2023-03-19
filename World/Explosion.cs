using Godot;
using System;

public partial class Explosion : Marker2D
{
    private float Strength = -0.8f;

    public override void _Ready()
    {
        GetNode<PaintingZone>("PaintingZone").SetStrength(Strength);
    }

    public void SetStrength(float strength)
    {
        Strength = strength;
        if(HasNode("PaintingZone")){
            GetNode<PaintingZone>("PaintingZone").SetStrength(Strength);
        }
    }

    public void Explode(){
        if(Strength > 0)
            ((ParticleProcessMaterial)GetNode<GpuParticles2D>("GPUParticles2D").ProcessMaterial).Color = new Color(0f, 0.9f, 1f, Math.Abs(Strength));
        else
            ((ParticleProcessMaterial)GetNode<GpuParticles2D>("GPUParticles2D").ProcessMaterial).Color = new Color(1f, 0.4f, 0.3f, Math.Abs(Strength));
        GetNode<AnimationPlayer>("AnimationPlayer").Play("start");
    }
}
