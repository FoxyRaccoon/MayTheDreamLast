using Godot;
using System;

public partial class Weapon : Marker2D
{
	private float Strength = 0.3f;

	public void SetStrength(float strength)
	{
		Strength = strength;
	}
	
	public void Attack(){
		GetNode<PaintingZone>("PaintingZone").SetStrength(Strength);
		GetNode<CollisionShape2D>("PaintingZone/CollisionShape2D").Disabled = false;
		((ParticleProcessMaterial)GetNode<GpuParticles2D>("GPUParticles2D").ProcessMaterial).Color = new Color(0f, 0.9f, 1f, Strength);
		GetNode<GpuParticles2D>("GPUParticles2D").Emitting = true;
		GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
	}

	public void NegativeAttack(){
		GetNode<PaintingZone>("PaintingZone").SetStrength(-Strength);
		GetNode<CollisionShape2D>("PaintingZone/CollisionShape2D").Disabled = false;
		((ParticleProcessMaterial)GetNode<GpuParticles2D>("GPUParticles2D").ProcessMaterial).Color = new Color(1f, 0f, 0f, Strength);
		GetNode<GpuParticles2D>("GPUParticles2D").Emitting = true;
		GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
	}

	public void StopAttack(){
		GetNode<CollisionShape2D>("PaintingZone/CollisionShape2D").Disabled = true;
		GetNode<GpuParticles2D>("GPUParticles2D").Emitting = false;
		GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Stop();
	}

	public void OnAudioFinished(){
		GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
	}
}
