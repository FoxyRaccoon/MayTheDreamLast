using Godot;
using System;
using System.Collections.Generic;

public partial class World : Node2D
{
	private PackedScene WallScene;
	private PackedScene DoorScene;
	private PackedScene NightmareScene;
	private PackedScene StarScene;
	private PackedScene EyeScene;
	private PackedScene StarboxScene;
	private PackedScene EyeboxScene;

	private bool HasDoor = false;
	private bool CollisionEnabled = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		WallScene = (PackedScene)ResourceLoader.Load("res://World/wall.tscn");
		DoorScene = (PackedScene)ResourceLoader.Load("res://World/door.tscn");
		NightmareScene = (PackedScene)ResourceLoader.Load("res://Foes/nightmare.tscn");
		StarScene = (PackedScene)ResourceLoader.Load("res://World/star.tscn");
		EyeScene = (PackedScene)ResourceLoader.Load("res://World/eye.tscn");
		StarboxScene = (PackedScene)ResourceLoader.Load("res://World/star_box.tscn");
		EyeboxScene = (PackedScene)ResourceLoader.Load("res://World/eye_box.tscn");
		TerrainGen(new Vector2(-40, -40), new Vector2(40, 40));
		GlobalData.SetTier(1+(GlobalData.GetDifficulty() / 5));
		if(GlobalData.GetNightmareLevel()){
			GetNode<CanvasModulate>("CanvasModulate").Color = new Color(0.6f, 0.2f, 0.2f);
		}else{
			GetNode<CanvasModulate>("CanvasModulate").Color = new Color(0.6f, 0.6f, 0.6f);
		}
		GetNode<MusicListener>("/root/MusicListener").SetNightmareLevel(GlobalData.GetNightmareLevel());
	}

	public void OnBodyExited(Node2D body)
	{
		if(body is Player && CollisionEnabled){
			((Player)body).WakeUp();
		}
	}

	public void DisableCollision()
	{
		CollisionEnabled = false;
	}

	public void TerrainGen(Vector2 beginPosition, Vector2 endPosition)
	{
		Random random = new Random();
		int size = (int)(endPosition.X - beginPosition.X);
		List<List<TerrainEnum>> terrain = new List<List<TerrainEnum>>(size);
		for(int i = 0; i < size; i+= 1 ){
			terrain.Add(new List<TerrainEnum>(size));
			for(int j = 0; j < size; j+= 1 ){
				if(i == 0 || j == 0 || i == size - 1 || j == size - 1){
					terrain[i].Add(TerrainEnum.Wall);
				}else if( i > size/2 - 5 && i < size/2 + 5 && j > size/2 - 5 && j < size/2 + 5){
					terrain[i].Add(TerrainEnum.None);
				}else{
					TerrainEnum top = terrain[i-1][j];
					TerrainEnum left = terrain[i][j-1];
					if(GlobalData.GetNightmareLevel()){
						if(top == TerrainEnum.Wall && left == TerrainEnum.Wall){
							if(random.Next(0, 100) == 0){
								terrain[i].Add(TerrainEnum.Wall);
							}else if(random.Next(0, 20) == 0){
								terrain[i].Add(TerrainEnum.Nightmare);
							}else{
								terrain[i].Add(TerrainEnum.None);
							}
						}else if(top == TerrainEnum.None && left == TerrainEnum.None && terrain[i-1][j+1] == TerrainEnum.None && terrain[i-1][j-1] == TerrainEnum.None){
							if(random.Next(0, 40) == 0){
								terrain[i].Add(TerrainEnum.Wall);
							}else if(random.Next(0, 800) == 0){
								terrain[i].Add(TerrainEnum.Eye);
							}else if(random.Next(0, 300) == 0){
								terrain[i].Add(TerrainEnum.Door);
							}else if(random.Next(0, 100) == 0){
								terrain[i].Add(TerrainEnum.Nightmare);
							}else if(random.Next(0, 200) == 0 && GlobalData.GetDifficulty() % 5 == 0){
								terrain[i].Add(TerrainEnum.Eyebox);
							}else{
								terrain[i].Add(TerrainEnum.None);
							}
						}else if(top == TerrainEnum.Door || left == TerrainEnum.Door || terrain[i-1][j-1] == TerrainEnum.Door || terrain[i-1][j+1] == TerrainEnum.Door){
							terrain[i].Add(TerrainEnum.None);
						}else{
							if(random.Next(0, 3) == 0){
								terrain[i].Add(TerrainEnum.Wall);
							}else if(random.Next(0, 100) == 0){
								terrain[i].Add(TerrainEnum.Nightmare);
							}else{
								terrain[i].Add(TerrainEnum.None);
							}
						}
					}else{
						if(top == TerrainEnum.Wall && left == TerrainEnum.Wall){
							if(random.Next(0, 20) == 0){
								terrain[i].Add(TerrainEnum.Wall);
							}else if(random.Next(0, 500) == 0){
								terrain[i].Add(TerrainEnum.Nightmare);
							}else{
								terrain[i].Add(TerrainEnum.None);
							}
						}else if(top == TerrainEnum.None && left == TerrainEnum.None && terrain[i-1][j+1] == TerrainEnum.None && terrain[i-1][j-1] == TerrainEnum.None){
							if(random.Next(0, 10) == 0){
								terrain[i].Add(TerrainEnum.Wall);
							}else if(random.Next(0, 800) == 0){
								terrain[i].Add(TerrainEnum.Star);
							}else if(random.Next(0, 300) == 0){
								terrain[i].Add(TerrainEnum.Door);
							}else if(random.Next(0, 500) == 0){
								terrain[i].Add(TerrainEnum.Nightmare);
							}else if(random.Next(0, 200) == 0 && GlobalData.GetDifficulty() % 5 == 0){
								terrain[i].Add(TerrainEnum.Starbox);
							}else{
								terrain[i].Add(TerrainEnum.None);
							}
						}else if(top == TerrainEnum.Door || left == TerrainEnum.Door || terrain[i-1][j-1] == TerrainEnum.Door || terrain[i-1][j+1] == TerrainEnum.Door){
							terrain[i].Add(TerrainEnum.None);
						}else{
							if(random.Next(0, 3) == 0){
								terrain[i].Add(TerrainEnum.Wall);
							}else if(random.Next(0, 500) == 0){
								terrain[i].Add(TerrainEnum.Nightmare);
							}else{
								terrain[i].Add(TerrainEnum.None);
							}
						}
					}
					
				}
			}
			
		}
		for(int i = size-1; i >= 0; i-= 1 ){
			for(int j = size-1; j >= 0; j-= 1 ){
				if(terrain[i][j] == TerrainEnum.Wall){
					AddWall(new Vector2(i*20, j*20) - new Vector2(size*10, size*10) + new Vector2(18, 18));
				}else if(terrain[i][j] == TerrainEnum.Door){
					AddDoor(new Vector2(i*20, j*20) - new Vector2(size*10, size*10) + new Vector2(10, 10));
				}else if(terrain[i][j] == TerrainEnum.Nightmare){
					AddNightmare(new Vector2(i*20, j*20) - new Vector2(size*10, size*10) + new Vector2(10, 10));
				}else if(terrain[i][j] == TerrainEnum.Star){
					AddStar(new Vector2(i*20, j*20) - new Vector2(size*10, size*10) + new Vector2(10, 10));
				}else if(terrain[i][j] == TerrainEnum.Eye){
					AddEye(new Vector2(i*20, j*20) - new Vector2(size*10, size*10) + new Vector2(10, 10));
				}else if(terrain[i][j] == TerrainEnum.Eyebox){
					AddEyebox(new Vector2(i*20, j*20) - new Vector2(size*10, size*10) + new Vector2(10, 10));
				}else if(terrain[i][j] == TerrainEnum.Starbox){
					AddStarbox(new Vector2(i*20, j*20) - new Vector2(size*10, size*10) + new Vector2(10, 10));
				}
			}
		}
		if(!HasDoor){
			AddDoor(new Vector2(10,80));
		}
	}

	private void AddDoor(Vector2 position)
	{
		Door instance = (Door)DoorScene.Instantiate();
		instance.GlobalPosition = position;
		GetNode("Terrain").AddChild(instance);
		HasDoor = true;
	}

	private void AddWall(Vector2 position)
	{
		Wall instance = (Wall)WallScene.Instantiate();
		instance.GlobalPosition = position;
		GetNode("Terrain").AddChild(instance);
	}

	private void AddNightmare(Vector2 position)
	{
		Nightmare instance = (Nightmare)NightmareScene.Instantiate();
		instance.GlobalPosition = position;
		GetNode("Entities").AddChild(instance);
	}

	private void AddStar(Vector2 position)
	{
		Star instance = (Star)StarScene.Instantiate();
		instance.GlobalPosition = position;
		GetNode("Entities").AddChild(instance);
	}

	private void AddEye(Vector2 position)
	{
		Eye instance = (Eye)EyeScene.Instantiate();
		instance.GlobalPosition = position;
		GetNode("Entities").AddChild(instance);
	}

	private void AddEyebox(Vector2 position)
	{
		EyeBox instance = (EyeBox)EyeboxScene.Instantiate();
		instance.GlobalPosition = position;
		GetNode("Entities").AddChild(instance);
	}

	private void AddStarbox(Vector2 position)
	{
		StarBox instance = (StarBox)StarboxScene.Instantiate();
		instance.GlobalPosition = position;
		GetNode("Entities").AddChild(instance);
	}

	public void OnTimeOut(){
		GlobalData.SetTier(GlobalData.GetTier() + 1);
	}
}
