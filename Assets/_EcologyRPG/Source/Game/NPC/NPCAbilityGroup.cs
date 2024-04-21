using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.GameSystems.NPC
{
    enum ChangeMode
    {
        Shuffle,
        Random
    }

    [CreateAssetMenu(fileName = "New NPC Ability Group", menuName = "Ability/NPC Ability Group")]
    public class NPCAbilityGroup : NPCAbilityReference
    {

        [SerializeField] ChangeMode changeMode;
        [SerializeField] NPCAbilityReference[] abilities;


        short currentIndex = 0;

        //public override void Initialize(BaseCharacter owner, AbilityDefintion prefabAbility)
        //{
        //    currentIndex = 0;
        //    for (int i = 0; i < abilities.Length; i++)
        //    {
        //        abilities[i] = Instantiate(abilities[i]);
        //        abilities[i].Initialize(owner, abilities[i]);
        //    }

        //    base.Initialize(owner, prefabAbility);
        //    Setup(abilities[currentIndex]);
        //}

        //void Setup(NPCAbility abilityData)
        //{
        //    Ability = abilityData.Ability;
        //    Cooldown = abilityData.Cooldown;
        //    CastWindupTime = abilityData.CastWindupTime;
        //    BuffsOnCast = abilityData.BuffsOnCast;
        //    CastWindUp = abilityData.CastWindUp;
        //    TriggerHash = abilityData.TriggerHash;
        //}

        //public override void PutOnCooldown()
        //{
        //    base.PutOnCooldown();
        //    if (changeMode == ChangeMode.Random)
        //    {
        //        currentIndex = (short)Random.Range(0, abilities.Length);
        //    }
        //    else
        //    {
        //        currentIndex = (short)((currentIndex + 1) % abilities.Length);
        //    }
        //    Setup(abilities[currentIndex]);
        //}
    }
}
