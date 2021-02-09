
public enum BattleEventsEnum
{
    ATTACK_PLAYER,
    ENEMY_DEAD,
    WAVE_END,
    LEVEL_WIN,
    PLAYER_DEAD
}

public class BattleEvents
{
    public BattleEventsEnum type;
    public float load;
}