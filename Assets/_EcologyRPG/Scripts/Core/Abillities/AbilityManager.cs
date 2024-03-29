using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public class AbilityManager
    {
        public static AbilityManager instance;

        public List<AbilityDefintion> CooldownAbilities = new List<AbilityDefintion>();

        public static void Init()
        {
            instance = new();
        }

        public void RegisterAbilityOnCooldown(AbilityDefintion ability)
        {
            if (!CooldownAbilities.Contains(ability))
            {
                CooldownAbilities.Add(ability);
            }
        }

        public void UnregisterAbilityOnCooldown(AbilityDefintion ability)
        {
            if (CooldownAbilities.Contains(ability))
            {
                CooldownAbilities.Remove(ability);
            }
        }

        public void Update()
        {
            for (int i = CooldownAbilities.Count - 1; i >= 0; i--)
            {
                if (CooldownAbilities[i] == null)
                {
                    CooldownAbilities.RemoveAt(i);
                    continue;
                }

                if (CooldownAbilities[i].state == AbilityStates.ready)
                {
                    CooldownAbilities.RemoveAt(i);
                    continue;
                }

                CooldownAbilities[i].UpdateCooldown(Time.deltaTime);
            }
        }
    }
}