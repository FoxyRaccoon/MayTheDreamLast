public class PlayerData{
    private static float Speed = 140.0f;
	private static float MaxCompleteness = 2.0f;
	private static int BombCount = 3;
	private static int TowerCount = 3;
	private static int UnicornCount = 1;
    private static int StarCount = 0;
    private static int EyeCount = 0;
	private static float WeaponStrength = 0.4f;
	private static float TowerStrength = 0.1f;
	private static float BombStrength = 0.8f;
    public static event StatChanged UIUpdate;
    public static event StatChanged SpeedChanged;
    public static event StatChanged WeaponStrengthChanged;
    public static event StatChanged ItemCountChanged;

    public static float GetSpeed(){
        return Speed;
    }
    public static float GetMaxCompleteness(){
        return MaxCompleteness;
    }
    public static int GetBombCount(){
        return BombCount;
    }
    public static int GetTowerCount(){
        return TowerCount;
    }
    public static int GetUnicornCount(){
        return UnicornCount;
    }
    public static int GetStarCount(){
        return StarCount;
    }
    public static int GetEyeCount(){
        return EyeCount;
    }
    public static float GetWeaponStrength(){
        return WeaponStrength;
    }
    public static float GetTowerStrength(){
        return TowerStrength;
    }
    public static float GetBombStrength(){
        return BombStrength;
    }

    public static void Init(){
        Speed = 140.0f;
        MaxCompleteness = 2.0f;
        BombCount = 3;
        TowerCount = 3;
        UnicornCount = 1;
        WeaponStrength = 0.4f;
        TowerStrength = 0.1f;
        BombStrength = 0.8f;
        StarCount = 0;
        EyeCount = 0;
    }

	public static void AddMaxCompleteness(float value){
		MaxCompleteness += value;
        if(MaxCompleteness < 1f){
            MaxCompleteness = 1f;
        }
	}
	public static void AddUnicornCount(int value){
		UnicornCount += value;
        if(UnicornCount < 0){
            UnicornCount = 0;
        }
	}
	public static void AddBombCount(int value){
		BombCount += value;
        if(BombCount < 0){
            BombCount = 0;
        }
        UIUpdate?.Invoke();
	}
	public static void AddTowerCount(int value){
		TowerCount += value;
        if(TowerCount < 0){
            TowerCount = 0;
        }
        UIUpdate?.Invoke();
	}
    public static void AddStarCount(int value){
        StarCount += value;
        if(StarCount < 0){
            StarCount = 0;
        }
        ItemCountChanged?.Invoke();
    }
    public static void AddEyeCount(int value){
        EyeCount += value;
        if(EyeCount < 0){
            EyeCount = 0;
        }
        ItemCountChanged?.Invoke();
    }
	public static void AddSpeed(float value){
		Speed += value;
        if(Speed < 40f){
            Speed = 40f;
        }
        SpeedChanged?.Invoke();
	}
	public static void AddWeaponStrength(float value){
		WeaponStrength += value;
        if(WeaponStrength < 0.1f){
            WeaponStrength = 0.1f;
        }
        WeaponStrengthChanged?.Invoke();
	}
	public static void AddTowerStrength(float value){
		TowerStrength += value;
        if(TowerStrength < 0.05f){
            TowerStrength = 0.05f;
        }
	}
	public static void AddBombStrength(float value){
		BombStrength += value;
        if(BombStrength < 0.3f){
            BombStrength = 0.3f;
        }
	}

    public static void ResetItems(){
        StarCount = 0;
        EyeCount = 0;
    }

    public static void DoubleStats(){
        Speed *= 2f;
        WeaponStrength *= 2f;
        TowerStrength *= 2f;
        BombStrength *= 2f;
        MaxCompleteness *= 2f;
        BombCount *= 2;
        TowerCount *= 2;
        UnicornCount *= 2;
    }
}