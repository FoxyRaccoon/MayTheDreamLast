using Godot;
using System.Collections.Generic;

public partial class PaintedZone : Area2D
{
    private float Completeness = PlayerData.GetMaxCompleteness();
	private List<PaintingZone> Areas = new List<PaintingZone>();
    private List<PaintingZone> AreasToExclude = new List<PaintingZone>();
    private CollisionShape2D Collision;
	private LightOccluder2D Occluder;
	private PointLight2D Light;
    private Node2D Sprite;

	public float GetCompleteness()
	{
		return Completeness;
	}

    public bool IsBeingPainted()
    {
        return Areas.Count > 0;
    }

    public void AddAreaToExclude(PaintingZone area)
    {
        AreasToExclude.Add(area);
        if(Areas.Contains(area)){
            Areas.Remove(area);
        }
    }

    public override void _Ready()
	{
        Collision = GetParent().GetNode<CollisionShape2D>("CollisionShape2D");
        Sprite = GetParent().GetNode<Node2D>("Sprite");
		if(GetParent() is Wall or Unicorn){
			Occluder = GetParent().GetNode<LightOccluder2D>("LightOccluder2D");
		}
		if(GetParent() is Eye or Star){
			Light = GetParent().GetNode<PointLight2D>("PointLight2D");
		}
	}

    public override void _PhysicsProcess(double delta)
	{
        float paintAmount = 0.0f;
		foreach(PaintingZone area in Areas){
			paintAmount += area.GetStrength();
		}
        Completeness += (float)(paintAmount*delta) - (float)(GlobalData.GetDifficulty()*0.01*delta);
        if(Completeness >= PlayerData.GetMaxCompleteness()){
				Completeness = PlayerData.GetMaxCompleteness();
        }else if(Completeness <= 0.0f){
            Completeness = 0.0f;
        }

        if(Completeness <= 0.4){
			Collision.Disabled = true;
			if(Occluder != null){
				Occluder.Visible = false;
			}
		}else{
			Collision.Disabled = false;
			if(Occluder != null){
				Occluder.Visible = true;
			}
		}

		if(Light != null){
			Light.Energy = Completeness*0.5f;
		}
		
		Sprite.Modulate = new Color(Sprite.Modulate, Completeness);
    }

	public void OnAreaEntered(Area2D area)
	{
		if(area is PaintingZone && !AreasToExclude.Contains((PaintingZone)area)){
			Areas.Add((PaintingZone)area);
		}
	}

	public void OnAreaExited(Area2D area)
	{
		if(area is PaintingZone && !AreasToExclude.Contains((PaintingZone)area)){
			Areas.Remove((PaintingZone)area);
		}
	}
}
