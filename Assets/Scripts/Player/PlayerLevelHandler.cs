using Player;
using System.Collections.Generic;

public class PlayerLevelHandler : PlayerModule
{
    PlayerCharacter player;
    List<float> xpRequiredPerLevel;
    float currentXp;

    public override void Initialize(PlayerCharacter player)
    {
        this.player = player;
        EventManager.AddListener("XP", OnXpGain);
        xpRequiredPerLevel = player.playerSettings.XpRequiredPerLevel;
    }

    void OnXpGain(object data)
    {
        if (data is float xp)
        {
            currentXp += xp;
            var xpRequired = xpRequiredPerLevel[player.Level - 1];
            if (currentXp >= xpRequired)
            {
                currentXp -= xpRequired;
                player.LevelUp();
            }
        }
    }

    public float GetXpPercantage()
    {
        return currentXp / xpRequiredPerLevel[player.Level - 1];
    }

    public override void OnDestroy()
    {

    }
}
