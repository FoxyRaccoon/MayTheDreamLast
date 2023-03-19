using Godot;

public partial class MusicListener : AudioStreamPlayer{
    private AudioStream DreamMusic = (AudioStream)ResourceLoader.Load("res://Resources/Music/Abstraction - Three Red Hearts - Princess Quest.wav");
    private AudioStream NightmareMusic = (AudioStream)ResourceLoader.Load("res://Resources/Music/Abstraction - Three Red Hearts - Box Jump.wav");
    public override void _Ready()
    {
        Stream = DreamMusic;
        Play();
    }

    public void SetNightmareLevel(bool nightmareLevel)
    {
        if(nightmareLevel && Stream != NightmareMusic){
            Stream = NightmareMusic;
            Play();
        }else if(!nightmareLevel && Stream != DreamMusic){
            Stream = DreamMusic;
            Play();
        }
    }

    public void OnFinished(){
        Play();
    }
}