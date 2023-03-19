using Godot;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public partial class EndMenu : Control
{
    private int SoundTimesPlayed = 0;
    private PackedScene ScoreLineScene = GD.Load<PackedScene>("res://Menus/score_line.tscn");
    private PackedScene NewScoreLineScene = GD.Load<PackedScene>("res://Menus/new_score_line.tscn");
    private AudioStream AlarmBroken = (AudioStream)ResourceLoader.Load("res://Resources/Sounds/Explosion 68.wav");
    private bool RequestScores = false;

    public override void _Ready()
    {
        GetNode<MusicListener>("/root/MusicListener").Stop();
        GetNode<Label>("MarginContainer/CenterContainer/HBoxContainer/MarginContainer/VBoxContainer/NewScoreLine/Score").Text = GlobalData.GetDifficulty().ToString();
        GetNode<HttpRequest>("HTTPRequest").RequestCompleted += OnHttpRequestCompleted;
        HttpRequest request = GetNode<HttpRequest>("HTTPRequest");
        request.Request("https://fierce-fox-beret.cyclic.app/scores");
        RequestScores = true;
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("exit")){
            if(GetNode<Control>("Credits").Visible){
                GetNode<Control>("Credits").Visible = false;
            }else{
                OnQuitPressed();
            }
        }
    }

    public void OnPlayAgainPressed(bool _pressed)
    {
        SoundTimesPlayed = 0;
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = AlarmBroken;
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        GetNode<MusicListener>("/root/MusicListener").Play();
        PlayerData.Init();
        GlobalData.Init();
        StarBox.ResetBoxOpened();
        EyeBox.ResetBoxOpened();
        GetNode<Transition>("/root/Transition").WhiteTransition("res://World/world.tscn");
    }

    public void OnFinished()
    {
        if(SoundTimesPlayed < 5){
            SoundTimesPlayed++;
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        }
    }

    public void OnCreditsPressed()
    {
        GetNode<Control>("Credits").Visible = true;
    }

    public void OnQuitPressed()
    {
        GetTree().Quit();
    }

    public void OnHttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        if(responseCode != 200){
            GD.Print("Error: " + responseCode);
            return;
        }else{
            Json jsonResponse = new Json();
            jsonResponse.Parse(Encoding.UTF8.GetString(body));
            if(RequestScores){
                Dictionary<string, int> labels = new Dictionary<string, int>();
                Godot.Collections.Array scores = ((Godot.Collections.Array)jsonResponse.Data);
                foreach(var score in scores){
                    string key = (string)((Godot.Collections.Dictionary)score)["key"];
                    int value = (int)((Godot.Collections.Dictionary)((Godot.Collections.Dictionary)score)["props"])["score"];
                    labels.Add(key, value);
                }
                foreach(var scoreLabel in GetNode<Control>("MarginContainer/CenterContainer/HBoxContainer/MarginContainer/VBoxContainer").GetChildren()){
                    if(scoreLabel is ScoreLine){
                        scoreLabel.QueueFree();
                    }
                }
                foreach(var score in labels.OrderByDescending(x => x.Value)){
                    ScoreLine newScoreLine = ScoreLineScene.Instantiate() as ScoreLine;
                    newScoreLine.SetScore(score.Key, score.Value);
                    GetNode<Control>("MarginContainer/CenterContainer/HBoxContainer/MarginContainer/VBoxContainer").AddChild(newScoreLine);
                }
                RequestScores = false;
            }else{
                RequestScores = true;
                GetNode<HttpRequest>("HTTPRequest").Request("https://fierce-fox-beret.cyclic.app/scores");
            }
        }
    }

    public void OnAddScoreEntered(string name)
    {
        int score = GlobalData.GetDifficulty();
        GetNode<HttpRequest>("HTTPRequest").Request("https://fierce-fox-beret.cyclic.app/scores", method: HttpClient.Method.Post, customHeaders: new string[]{"Content-Type: application/json"}, requestData: "{\"name\": \"" + name + "\",  \"score\":" + score + "}");
        GetNode<Control>("MarginContainer/CenterContainer/HBoxContainer/MarginContainer/VBoxContainer/NewScoreLine").Visible = false;
    }
}
