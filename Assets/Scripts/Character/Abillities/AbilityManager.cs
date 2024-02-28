using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    public List<BaseAbility> CooldownAbilities = new List<BaseAbility>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void RegisterAbilityOnCooldown(BaseAbility ability)
    {
        if (!CooldownAbilities.Contains(ability))
        {
            CooldownAbilities.Add(ability);
        }
    }

    public void UnregisterAbilityOnCooldown(BaseAbility ability)
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
            }

            if (CooldownAbilities[i].state == AbilityStates.ready)
            {
                CooldownAbilities.RemoveAt(i);
            }

            if (CooldownAbilities[i].state == AbilityStates.cooldown)
            {
                CooldownAbilities[i].remainingCooldown -= TimeManager.IngameDeltaTime;
                if (CooldownAbilities[i].remainingCooldown <= 0)
                {
                    CooldownAbilities[i].remainingCooldown = 0;
                    CooldownAbilities[i].state = AbilityStates.ready;
                    CooldownAbilities.RemoveAt(i);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if(instance == this)
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