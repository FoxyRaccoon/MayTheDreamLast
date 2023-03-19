using Godot;
using System.Collections.Generic;

public partial class PaintingZone : Area2D{
    private float Strength = 0.0f;

	public float GetStrength()
	{
		return Strength;
	}

	public void SetStrength(float strength)
	{
		Strength = strength;
	}
}