using Godot;
using System;

public partial class Wall : StaticBody2D
{
	public float GetCompleteness()
	{
		return GetNode<PaintedZone>("PaintedZone").GetCompleteness();
	}

}
