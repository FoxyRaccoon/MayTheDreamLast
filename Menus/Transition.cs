using Godot;
using System;

public partial class Transition : CanvasLayer
{
    private bool Loaded;
    private string Scene;
    
    public void WhiteTransition(string scene)
    {
        Scene = scene;
        Loaded = true;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("start");
    }

    public void EndWhiteTransition()
    {   
        if(Loaded){
            GetTree().ChangeSceneToFile(Scene);
            GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("start");
            Loaded = false;
        }
    }

    public void BlackTransition(string scene)
    {
        Scene = scene;
        Loaded = true;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("lose");
    }

    public void EndBlackTransition()
    {   
        if(Loaded){
            GetTree().ChangeSceneToFile(Scene);
            GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("lose");
            Loaded = false;
        }
    }

    public void BlueTransition(string scene)
    {
        Scene = scene;
        Loaded = true;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("blue");
    }

    public void EndBlueTransition()
    {   
        if(Loaded){
            GetTree().ChangeSceneToFile(Scene);
            GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("blue");
            Loaded = false;
        }
    }

    public void GreenTransition(string scene)
    {
        Scene = scene;
        Loaded = true;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("green");
    }

    public void EndGreenTransition()
    {   
        if(Loaded){
            GetTree().ChangeSceneToFile(Scene);
            GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("green");
            Loaded = false;
        }
    }
}
