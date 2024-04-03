
using EcologyRPG.Core.Character;
using System.Collections.Generic;

namespace EcologyRPG.Game.Player
{
    public class PlayerCharacter : BaseCharacter
    {
        public PlayerSettings playerSettings;
        List<float> xpRequiredPerLevel;
        float currentXp;


        public PlayerCharacter(PlayerSettings playerSettings) : base()
        {
            this.playerSettings = playerSettings;
            faction = Faction.player;
            Tags = playerSettings.Tags;
            EventManager.AddListener("XP", OnXpGain);
            xpRequiredPerLevel = playerSettings.XpRequiredPerLevel;
            currentXp = 0;

        }

        void OnXpGain(EventData data)
        {
            if (data is DefaultEventData eventData)
            {
                if (eventData.data is float xp)
                {
                    currentXp += xp;
                    var xpRequired = xpRequiredPerLevel[Level - 1];
                    if (currentXp >= xpRequired)
                    {
                        currentXp -= xpRequired;
                        LevelUp();
                    }
                }
            }
        }

        public void Respawn()
        {
            state = CharacterStates.active;
            Health.CurrentValue = Health.MaxValue;
        }

        public override void Die()
        {
            base.Die();
            PlayerManager.Instance.PlayerDead();
        }

        public virtual void LevelUp()
        {
            level++;
            foreach (var mod in levelMods)
            {
                mod.Value = level;
            }
        }

        public override void Update()
        {
            if (IsPaused) return;
            base.Update();
        }
    }

}