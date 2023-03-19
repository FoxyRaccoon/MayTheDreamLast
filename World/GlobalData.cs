using Godot;
public delegate void StatChanged();
public partial class GlobalData 
{
    private static int Difficulty = 1;
    private static int Tier = 1;
    private static bool NightmareLevel = false;
    public static event StatChanged UIUpdate;

    public static void Init()
    {
        Difficulty = 1;
        Tier = 1;
        NightmareLevel = false;
    }
    public static int GetDifficulty()
    {
        return Difficulty;
    }
    public static void SetDifficulty(int difficulty)
    {
        Difficulty = difficulty;
        UIUpdate?.Invoke();
    }
    public static int GetTier()
    {
        return Tier;
    }
    public static void SetTier(int tier)
    {
        Tier = tier;
        UIUpdate?.Invoke();
    }
    public static bool GetNightmareLevel()
    {
        return NightmareLevel;
    }
    public static void SetNightmareLevel(bool nightmareLevel)
    {
        NightmareLevel = nightmareLevel;
    }
}
