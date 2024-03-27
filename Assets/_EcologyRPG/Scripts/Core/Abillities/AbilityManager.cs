using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager instance;

        public List<AbilityDefintion> CooldownAbilities = new List<AbilityDefintion>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
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

        private void Update()
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

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
            foreach (var ability in CooldownAbilities)
            {
                ability.remainingCooldown = 0;
                ability.state = AbilityStates.ready;
            }
        }
    }
}